"""
Gardony Map Drawing Analyzer (GMDA) - Python Implementation

This module provides tools for analyzing sketch maps in spatial cognition research.
It allows quantitative assessment of hand-drawn sketch maps compared to reference maps,
providing metrics for spatial accuracy and configurational characteristics.

The analysis includes both categorical measures (North/South, East/West judgments) 
and quantitative measures such as bidimensional regression.
"""

import numpy as np
import pandas as pd
import os
import math
from pathlib import Path


def read_input_files(reference_file, sketch_file):
    """
    Read reference and sketch map coordinates from CSV files.
    
    Parameters
    ----------
    reference_file : str
        Path to reference map CSV file with columns: landmark_name, x_coordinate, y_coordinate
    sketch_file : str
        Path to sketch map CSV file with columns: landmark_name, x_coordinate, y_coordinate
        The file may optionally contain an is_missing column
        
    Returns
    -------
    ref_data : pandas.DataFrame
        Reference map coordinates
    sketch_data : pandas.DataFrame
        Sketch map coordinates
        
    Raises
    ------
    ValueError
        If landmark names don't match between reference and sketch files
    """
    ref_data = pd.read_csv(reference_file)
    sketch_data = pd.read_csv(sketch_file)
    
    # Check if is_missing column exists, create it if it doesn't
    if 'is_missing' not in sketch_data.columns:
        sketch_data['is_missing'] = pd.NA
    
    # Process missing landmarks
    sketch_data['is_missing'] = sketch_data['is_missing'].fillna('')
    sketch_data['is_missing'] = sketch_data['is_missing'].apply(lambda x: True if x == 'y' else False)
    
    # Ensure landmark names are aligned
    if not all(ref_data['landmark_name'].values == sketch_data['landmark_name'].values):
        raise ValueError("Landmark names in reference and sketch files must match")
    
    return ref_data, sketch_data


def generate_combinations(landmark_count):
    """
    Generate all pairwise landmark combinations.
    
    Parameters
    ----------
    landmark_count : int
        Number of landmarks
        
    Returns
    -------
    combinations : list of tuples
        All pairwise landmark combinations (i, j) where i < j
    """
    combinations = []
    for i in range(landmark_count):
        for j in range(i+1, landmark_count):
            combinations.append((i, j))
    return combinations


def generate_canonical_judgments(combinations, ref_data):
    """
    Generate canonical N/S, E/W judgments for all landmark pairs.
    
    Parameters
    ----------
    combinations : list of tuples
        All pairwise landmark combinations
    ref_data : pandas.DataFrame
        Reference map coordinates
        
    Returns
    -------
    canonical_judgments : dict
        Dictionary with NS and EW judgments for each landmark pair
    """
    canonical_judgments = {'NS': [], 'EW': []}
    
    for i, j in combinations:
        x1, y1 = ref_data.loc[i, 'x_coordinate'], ref_data.loc[i, 'y_coordinate']
        x2, y2 = ref_data.loc[j, 'x_coordinate'], ref_data.loc[j, 'y_coordinate']
        
        # N/S Judgment
        if y1 <= y2:
            if y1 == y2:
                ns_judgment = "F"  # Flat (no difference)
            else:
                ns_judgment = "S"  # South
        else:
            ns_judgment = "N"  # North
            
        # E/W Judgment
        if x1 <= x2:
            if x1 == x2:
                ew_judgment = "F"  # Flat (no difference)
            else:
                ew_judgment = "W"  # West
        else:
            ew_judgment = "E"  # East
            
        canonical_judgments['NS'].append(ns_judgment)
        canonical_judgments['EW'].append(ew_judgment)
    
    return canonical_judgments


def calculate_distances_and_angles(combinations, ref_data, sketch_data=None):
    """
    Calculate distances, distance ratios, and angles between landmark pairs.
    
    Parameters
    ----------
    combinations : list of tuples
        All pairwise landmark combinations
    ref_data : pandas.DataFrame
        Reference map coordinates
    sketch_data : pandas.DataFrame, optional
        Sketch map coordinates - used to check for missing landmarks
        
    Returns
    -------
    distances : dict
        Dictionary with distances, distance ratios, and angles
    """
    distances = {
        'distance': [],
        'angle': []
    }
    
    valid_distances = []  # Only includes distances where both landmarks exist in sketch map
    
    for i, j in combinations:
        x1, y1 = ref_data.loc[i, 'x_coordinate'], ref_data.loc[i, 'y_coordinate']
        x2, y2 = ref_data.loc[j, 'x_coordinate'], ref_data.loc[j, 'y_coordinate']
        
        # Calculate Euclidean distance
        distance = np.sqrt((x2 - x1)**2 + (y2 - y1)**2)
        
        # Calculate angle using arctan2 (returns angle in radians)
        angle = np.degrees(np.arctan2(x2 - x1, y2 - y1))
        
        distances['distance'].append(distance)
        distances['angle'].append(angle)
        
        # Track valid distances (where both landmarks exist in sketch map)
        if sketch_data is not None:
            if not (sketch_data.loc[i, 'is_missing'] or sketch_data.loc[j, 'is_missing']):
                valid_distances.append(distance)
    
    # Calculate distance ratios using only valid distances for max_distance
    if sketch_data is not None and valid_distances:
        max_distance = max(valid_distances)
    else:
        max_distance = max(distances['distance']) if distances['distance'] else 1.0
        
    distances['distance_ratio'] = [d / max_distance for d in distances['distance']]
    
    return distances


def compare_landmarks(i, j, sketch_data):
    """
    Compare two landmarks on sketch map using categorical NSEW measures and 
    quantitative distance and angle measures.
    
    Parameters
    ----------
    i, j : int
        Indices of landmarks to compare
    sketch_data : pandas.DataFrame
        Sketch map coordinates
        
    Returns
    -------
    comparisons : dict
        Dictionary with comparison results including NS, EW judgments, distance, and angle
    """
    comparisons = {}
    
    # Check if landmarks are missing
    if sketch_data.loc[i, 'is_missing'] or sketch_data.loc[j, 'is_missing']:
        comparisons['NS'] = "M"
        comparisons['EW'] = "M"
        comparisons['distance'] = "M"
        comparisons['distance_ratio'] = "M"
        comparisons['angle'] = "M"
        return comparisons
    
    x1, y1 = sketch_data.loc[i, 'x_coordinate'], sketch_data.loc[i, 'y_coordinate']
    x2, y2 = sketch_data.loc[j, 'x_coordinate'], sketch_data.loc[j, 'y_coordinate']
    
    # Canonical Comparison
    # N/S comparison
    if y1 > y2:
        comparisons['NS'] = "N"
    elif y1 < y2:
        comparisons['NS'] = "S"
    else:
        comparisons['NS'] = "F"  # Flat
    
    # E/W comparison
    if x1 > x2:
        comparisons['EW'] = "E"
    elif x1 < x2:
        comparisons['EW'] = "W"
    else:
        comparisons['EW'] = "F"  # Flat
    
    # Calculate Euclidean distance
    distance = np.sqrt((x2 - x1)**2 + (y2 - y1)**2)
    
    # Calculate angle using arctan2 (returns angle in radians)
    angle = np.degrees(np.arctan2(x2 - x1, y2 - y1))
    
    comparisons['distance'] = distance
    comparisons['angle'] = angle
    
    return comparisons


def compute_BDR(ref_data, sketch_data, dependent_is_sketch=True):
    """
    Compute bidimensional regression parameters.
    
    This function calculates the bidimensional regression parameters between 
    reference and sketch maps, which quantify the similarity and transformation 
    between the two spatial configurations.
    
    Parameters
    ----------
    ref_data : pandas.DataFrame
        Reference map coordinates
    sketch_data : pandas.DataFrame
        Sketch map coordinates
    dependent_is_sketch : bool, default=True
        If True, sketch map is the dependent variable; otherwise, reference map is
        
    Returns
    -------
    bdr_params : dict or None
        Dictionary with BDR parameters, or None if calculation not possible
    a_prime_b_prime : pandas.DataFrame or None
        Predicted coordinates, or None if calculation not possible
    """
    # Filter out missing landmarks
    valid_indices = ~sketch_data['is_missing']
    
    if valid_indices.sum() < 2:
        return None, None  # Not enough valid landmarks
    
    # Prepare arrays for computation
    X = ref_data.loc[valid_indices, 'x_coordinate'].values
    Y = ref_data.loc[valid_indices, 'y_coordinate'].values
    A = sketch_data.loc[valid_indices, 'x_coordinate'].values
    B = sketch_data.loc[valid_indices, 'y_coordinate'].values
    
    # Swap if reference map is dependent variable
    if not dependent_is_sketch:
        X, A = A, X
        Y, B = B, Y
    
    # Compute averages
    mean_X = np.mean(X)
    mean_Y = np.mean(Y)
    mean_A = np.mean(A)
    mean_B = np.mean(B)
    
    # Compute sum of squares
    sum_sq_X = np.sum((X - mean_X)**2)
    sum_sq_Y = np.sum((Y - mean_Y)**2)
    sum_sq_A = np.sum((A - mean_A)**2)
    sum_sq_B = np.sum((B - mean_B)**2)
    
    sum_sq_X_and_Y = sum_sq_X + sum_sq_Y
    sum_sq_A_and_B = sum_sq_A + sum_sq_B
    
    # Compute sum of products
    sum_products_X_and_A = np.sum((X - mean_X) * (A - mean_A))
    sum_products_Y_and_B = np.sum((Y - mean_Y) * (B - mean_B))
    sum_products_X_and_B = np.sum((X - mean_X) * (B - mean_B))
    sum_products_Y_and_A = np.sum((Y - mean_Y) * (A - mean_A))
    
    # Compute euclidean BDR parameters
    beta1 = (sum_products_X_and_A + sum_products_Y_and_B) / sum_sq_X_and_Y
    beta2 = (sum_products_X_and_B - sum_products_Y_and_A) / sum_sq_X_and_Y
    scale_factor = np.sqrt(beta1**2 + beta2**2)
    
    # Calculate theta using arctan2 function
    theta = np.arctan2(beta2, beta1)  
    if beta1 < 0:
        theta = theta + np.pi
    theta_degrees = np.degrees(theta)
    
    alpha1 = mean_A - beta1 * mean_X + beta2 * mean_Y
    alpha2 = mean_B - beta2 * mean_X - beta1 * mean_Y
    
    # Compute predicted coordinates
    A_prime = alpha1 + beta1 * X - beta2 * Y
    B_prime = alpha2 + beta2 * X + beta1 * Y
    
    # Compute sum of squares
    sum_sq_A_prime = np.sum((A_prime - mean_A)**2)
    sum_sq_B_prime = np.sum((B_prime - mean_B)**2)
    sum_sq_A_prime_and_B_prime = sum_sq_A_prime + sum_sq_B_prime
    
    # Compute r
    r = np.sqrt(sum_sq_A_prime_and_B_prime / sum_sq_A_and_B)
    
    # Compute distortion parameters
    dMax = np.sqrt(sum_sq_A_and_B)
    d = np.sqrt(sum_sq_A_and_B - sum_sq_A_prime_and_B_prime)
    di = 100.0 * (d / dMax)
    
    # Package results
    bdr_params = {
        'r': r,
        'alpha1': alpha1,
        'alpha2': alpha2,
        'beta1': beta1,
        'beta2': beta2,
        'scale': scale_factor,
        'theta': theta_degrees,
        'DMax': dMax,
        'D': d,
        'DI': di
    }
    
    # Create DataFrame for predicted coordinates
    landmark_indices = np.where(valid_indices)[0]
    a_prime_b_prime = pd.DataFrame({
        'landmark_index': landmark_indices,
        'A_prime': A_prime,
        'B_prime': B_prime
    })
    
    return bdr_params, a_prime_b_prime


def calculate_gmda_measures(combinations, canonical_judgments, ref_distances, ref_data, sketch_data, comparisons):
    """
    Calculate GMDA measures for map analysis.
    
    Parameters
    ----------
    combinations : list of tuples
        All pairwise landmark combinations
    canonical_judgments : dict
        Dictionary with NS and EW judgments for each landmark pair
    ref_distances : dict
        Dictionary with distances, distance ratios, and angles for reference map
    ref_data : pandas.DataFrame
        Reference map coordinates
    sketch_data : pandas.DataFrame
        Sketch map coordinates
    comparisons : list of dict
        Comparison results for each landmark pair
        
    Returns
    -------
    gmda_measures : dict
        GMDA measures for the entire map
    landmark_measures : dict
        GMDA measures for individual landmarks
    """
    num_correct = 0
    num_missing = 0
    num_distance_correct = 0
    distance_sum = 0.0
    cos_sum = 0.0
    sin_sum = 0.0
    abs_distance_sum = 0.0
    abs_angle_sum = 0.0
    
    # Initialize per-landmark measures
    n_landmarks = sketch_data.shape[0]
    landmark_data = {}
    
    for i in range(n_landmarks):
        if not sketch_data.loc[i, 'is_missing']:
            landmark_name = sketch_data.loc[i, 'landmark_name']
            landmark_data[i] = {
                'name': landmark_name,
                'canonical_correct': 0,
                'canonical_total': 0,
                'distance_diff': 0.0,
                'abs_distance_diff': 0.0,
                'sin_sum': 0.0,
                'cos_sum': 0.0,
                'abs_angle_sum': 0.0,
                'comparisons': 0
            }
    
    # Process all comparisons
    for idx, (i, j) in enumerate(combinations):
        comp = comparisons[idx]
        
        # Canonical judgment comparison (NS, EW)
        if comp['NS'] != "M":
            # Both landmarks are present
            # Count both landmarks' canonical judgments
            landmark_data[i]['canonical_total'] += 2
            landmark_data[j]['canonical_total'] += 2
            
            # NS comparison
            if comp['NS'] == canonical_judgments['NS'][idx]:
                num_correct += 1
                landmark_data[i]['canonical_correct'] += 1
                landmark_data[j]['canonical_correct'] += 1
            
            # EW comparison
            if comp['EW'] == canonical_judgments['EW'][idx]:
                num_correct += 1
                landmark_data[i]['canonical_correct'] += 1
                landmark_data[j]['canonical_correct'] += 1
            
            # Distance and angle comparison
            sketch_distance_ratio = float(comp['distance_ratio'])
            ref_distance_ratio = ref_distances['distance_ratio'][idx]
            
            # Calculate O-A Distance Ratio
            o_minus_a_distance = sketch_distance_ratio - ref_distance_ratio
            abs_o_minus_a_distance = abs(o_minus_a_distance)
            
            # Calculate angle differences
            observed_angle = float(comp['angle'])
            actual_angle = ref_distances['angle'][idx]
            o_minus_a_angle = observed_angle - actual_angle
            
            # Convert to radians for sin/cos calculations
            angle_rad = np.radians(o_minus_a_angle)
            sin_value = np.sin(angle_rad)
            cos_value = np.cos(angle_rad)
            
            # Convert angle differences to ±180° scale
            if o_minus_a_angle <= -180.0:
                o_minus_a_angle = 360.0 + o_minus_a_angle
            if o_minus_a_angle >= 180.0:
                o_minus_a_angle = -(360.0 - o_minus_a_angle)
            
            abs_o_minus_a_angle = abs(o_minus_a_angle)
            
            # Update global measures
            distance_sum += o_minus_a_distance
            abs_distance_sum += abs_o_minus_a_distance
            sin_sum += sin_value
            cos_sum += cos_value
            abs_angle_sum += abs_o_minus_a_angle
            num_distance_correct += 1
            
            # Update per-landmark measures
            for landmark_idx in [i, j]:
                landmark_data[landmark_idx]['distance_diff'] += o_minus_a_distance
                landmark_data[landmark_idx]['abs_distance_diff'] += abs_o_minus_a_distance
                landmark_data[landmark_idx]['sin_sum'] += sin_value
                landmark_data[landmark_idx]['cos_sum'] += cos_value
                landmark_data[landmark_idx]['abs_angle_sum'] += abs_o_minus_a_angle
                landmark_data[landmark_idx]['comparisons'] += 1
        else:
            # One or both landmarks are missing
            num_missing += 2
    
    # Calculate global GMDA measures
    num_comparisons = len(combinations) * 2  # Each comparison yields 2 judgments (NS, EW)
    
    # Calculate configural measures
    percent_correct = num_correct / num_comparisons
    # For percent_observed_correct, exclude missing landmarks
    observed_comparisons = num_comparisons - num_missing
    percent_observed_correct = num_correct / observed_comparisons if observed_comparisons > 0 else 0
    
    # Calculate scaling bias - average of distance ratio differences
    scaling_bias = distance_sum / num_distance_correct if num_distance_correct > 0 else 0
    
    # Calculate rotational bias using atan2
    y_bar = sin_sum / num_distance_correct if num_distance_correct > 0 else 0
    x_bar = cos_sum / num_distance_correct if num_distance_correct > 0 else 0
    rotational_bias = np.degrees(np.arctan2(y_bar, x_bar))
    
    # Calculate distance accuracy - 1 minus average of absolute distance ratio differences
    distance_accuracy = 1.0 - (abs_distance_sum / num_distance_correct) if num_distance_correct > 0 else 0
    angle_accuracy = 1.0 - ((abs_angle_sum / num_distance_correct) / 180.0) if num_distance_correct > 0 else 0
    
    # Count missing landmarks
    missing_landmarks = sketch_data['is_missing'].sum()
    
    # Package configural measures
    gmda_measures = {
        'Canonical Organization': percent_correct,
        'SQRT(Canonical Organization)': np.sqrt(percent_correct),
        'Canonical Accuracy': percent_observed_correct,
        'Num Landmarks Missing': missing_landmarks,
        'Scaling Bias': scaling_bias,
        'Rotational Bias': rotational_bias,
        'Distance Accuracy': distance_accuracy,
        'Angle Accuracy': angle_accuracy
    }
    
    # Calculate per-landmark measures
    landmark_measures = {}
    
    for idx, data in landmark_data.items():
        if data['comparisons'] > 0:
            landmark_name = data['name']
            
            # Calculate individual measures
            can_accuracy = data['canonical_correct'] / data['canonical_total'] if data['canonical_total'] > 0 else 0
            scaling_bias = data['distance_diff'] / data['comparisons']
            
            # Use atan2 for rotational bias calculation
            y_bar = data['sin_sum'] / data['comparisons']
            x_bar = data['cos_sum'] / data['comparisons']
            rot_bias = np.degrees(np.arctan2(y_bar, x_bar))
            
            distance_acc = 1.0 - (data['abs_distance_diff'] / data['comparisons'])
            angle_acc = 1.0 - ((data['abs_angle_sum'] / data['comparisons']) / 180.0)
            
            landmark_measures[landmark_name] = {
                'Canonical Accuracy': can_accuracy,
                'Scaling Bias': scaling_bias,
                'Rotational Bias': rot_bias,
                'Distance Accuracy': distance_acc,
                'Angle Accuracy': angle_acc
            }
    
    return gmda_measures, landmark_measures


def analyze_maps(reference_file, sketch_file, output_prefix=None):
    """
    Analyze reference and sketch maps and generate output files.
    
    This is the main function that orchestrates the entire analysis process.
    
    Parameters
    ----------
    reference_file : str
        Path to reference map CSV file
    sketch_file : str
        Path to sketch map CSV file
    output_prefix : str, optional
        Prefix for output files. If None, uses sketch file name without extension.
        
    Returns
    -------
    gmda_measures : dict
        GMDA measures
    bdr_params : dict
        Bidimensional regression parameters
    """
    # Read input files
    ref_data, sketch_data = read_input_files(reference_file, sketch_file)
    
    # Define output prefix
    if output_prefix is None:
        output_prefix = Path(sketch_file).stem
    
    # Generate combinations
    n_landmarks = len(ref_data)
    combinations = generate_combinations(n_landmarks)
    
    # Generate canonical judgments
    canonical_judgments = generate_canonical_judgments(combinations, ref_data)
    
    # Calculate distances and angles for reference map, accounting for missing landmarks
    ref_distances = calculate_distances_and_angles(combinations, ref_data, sketch_data)
    
    # Compare landmarks and calculate distances for sketch map
    comparisons = []
    sketch_distances = []
    
    for i, j in combinations:
        comp = compare_landmarks(i, j, sketch_data)
        comparisons.append(comp)
        
        # If distance is not "M", add it to the list of sketch distances
        if comp['distance'] != "M":
            sketch_distances.append(comp['distance'])
    
    # Calculate maximum distance in sketch map
    max_sketch_distance = max(sketch_distances) if sketch_distances else 1.0
    
    # Calculate distance ratios for sketch map using max_sketch_distance
    for comp in comparisons:
        if comp['distance'] != "M":
            comp['distance_ratio'] = comp['distance'] / max_sketch_distance
    
    # Compute bidimensional regression
    bdr_params, a_prime_b_prime = compute_BDR(ref_data, sketch_data)
    
    # Calculate GMDA measures
    if bdr_params is not None:
        gmda_measures, landmark_measures = calculate_gmda_measures(
            combinations, canonical_judgments, ref_distances, ref_data, sketch_data, comparisons)
    else:
        gmda_measures = None
        landmark_measures = None
    
    # Generate output files
    generate_summary_file(output_prefix, gmda_measures, bdr_params, landmark_measures, ref_data, sketch_data)
    generate_raw_file(output_prefix, combinations, canonical_judgments, ref_distances, comparisons, 
                      ref_data, sketch_data, a_prime_b_prime)
    
    return gmda_measures, bdr_params


def generate_summary_file(output_prefix, gmda_measures, bdr_params, landmark_measures, ref_data, sketch_data):
    """
    Generate summary output file with analysis results.
    
    Parameters
    ----------
    output_prefix : str
        Prefix for output file
    gmda_measures : dict
        GMDA measures
    bdr_params : dict
        Bidimensional regression parameters
    landmark_measures : dict
        GMDA measures for individual landmarks
    ref_data : pandas.DataFrame
        Reference map coordinates
    sketch_data : pandas.DataFrame
        Sketch map coordinates
    """
    output_file = f"{output_prefix}_summary.csv"
    
    with open(output_file, 'w') as f:
        f.write("Measure Type Key:\n\n")
        f.write("GMDA_c - Configural GMDA Measures\n")
        f.write("BDR_c - Configural BDR parameters\n")
        f.write("GMDA_i - GMDA Measures (individual landmarks)\n\n")
        
        f.write("Configural GMDA Measures\n\n")
        f.write("Measure Type,Filename,Measure,Score\n")
        
        if gmda_measures is not None:
            for measure, value in gmda_measures.items():
                f.write(f"GMDA_c,{output_prefix},{measure},{value}\n")
        
        f.write("\nConfigural BDR Parameters\n\n")
        
        if bdr_params is not None:
            f.write("Measure Type,Filename,Measure,Score\n")
            for param, value in bdr_params.items():
                f.write(f"BDR_c,{output_prefix},{param},{value}\n")
            
            f.write("\nGMDA measures for individual landmarks\n\n")
            f.write("Measure Type,Filename,Landmark,Measure,Score\n")
            
            for landmark, measures in landmark_measures.items():
                for measure, value in measures.items():
                    f.write(f"GMDA_i,{output_prefix},{landmark},{measure},{value}\n")
        else:
            f.write("Unable to calculate BDR parameters\n")


def generate_raw_file(output_prefix, combinations, canonical_judgments, ref_distances, comparisons, 
                     ref_data, sketch_data, a_prime_b_prime):
    """
    Generate raw output file with detailed analysis data.
    
    Parameters
    ----------
    output_prefix : str
        Prefix for output file
    combinations : list of tuples
        All pairwise landmark combinations
    canonical_judgments : dict
        Dictionary with NS and EW judgments for each landmark pair
    ref_distances : dict
        Dictionary with distances, distance ratios, and angles for reference map
    comparisons : list of dict
        Comparison results for each landmark pair
    ref_data : pandas.DataFrame
        Reference map coordinates
    sketch_data : pandas.DataFrame
        Sketch map coordinates
    a_prime_b_prime : pandas.DataFrame
        Predicted coordinates
    """
    output_file = f"{output_prefix}_raw.csv"
    
    with open(output_file, 'w') as f:
        f.write("Source,Target,Actual(N/S),Actual(E/W),Observed(N/S),Observed(E/W),Actual D,"
                "Actual D ratio,Actual Angle,Observed D,Observed D ratio,Observed Angle,"
                "O-A Distance Ratio,O-A Angle,ABS(O-A) Distance Ratio,ABS(O-A) Angle\n")
        
        for idx, (i, j) in enumerate(combinations):
            comp = comparisons[idx]
            
            source_name = ref_data.loc[i, 'landmark_name']
            target_name = ref_data.loc[j, 'landmark_name']
            
            actual_ns = canonical_judgments['NS'][idx]
            actual_ew = canonical_judgments['EW'][idx]
            
            observed_ns = comp['NS']
            observed_ew = comp['EW']
            
            actual_d = ref_distances['distance'][idx]
            actual_d_ratio = ref_distances['distance_ratio'][idx]
            actual_angle = ref_distances['angle'][idx]
            
            if observed_ns == "M":
                observed_d = "0.0"
                observed_d_ratio = "M"
                observed_angle = "M"
                o_a_dist_ratio = "0.0"
                o_a_angle = "0.0"
                abs_o_a_dist_ratio = "0.0"
                abs_o_a_angle = "0.0"
            else:
                observed_d = comp['distance']
                observed_d_ratio = comp['distance_ratio']
                observed_angle = comp['angle']
                
                # Calculate difference measures
                o_a_dist_ratio = float(observed_d_ratio) - actual_d_ratio
                o_a_angle = float(observed_angle) - actual_angle
                
                # Adjust angle to ±180 scale
                if o_a_angle <= -180.0:
                    o_a_angle = 360 + o_a_angle
                if o_a_angle >= 180.0:
                    o_a_angle = -(360 - o_a_angle)
                    
                abs_o_a_dist_ratio = abs(o_a_dist_ratio)
                abs_o_a_angle = abs(o_a_angle)
            
            f.write(f"{source_name},{target_name},{actual_ns},{actual_ew},{observed_ns},{observed_ew},"
                   f"{actual_d},{actual_d_ratio},{actual_angle},{observed_d},{observed_d_ratio},{observed_angle},"
                   f"{o_a_dist_ratio},{o_a_angle},{abs_o_a_dist_ratio},{abs_o_a_angle}\n")
        
        # Write Independent, Dependent, and Predicted values
        if a_prime_b_prime is not None:
            f.write("\nLandmark,Independent,,Dependent,,Predicted\n")
            f.write(",X,Y,A,B,A',B'\n")
            
            for _, row in a_prime_b_prime.iterrows():
                i = int(row['landmark_index'])
                
                x = ref_data.loc[i, 'x_coordinate']
                y = ref_data.loc[i, 'y_coordinate']
                a = sketch_data.loc[i, 'x_coordinate']
                b = sketch_data.loc[i, 'y_coordinate']
                a_prime = row['A_prime']
                b_prime = row['B_prime']
                
                landmark_name = ref_data.loc[i, 'landmark_name']
                
                f.write(f"{landmark_name},{x},{y},{a},{b},{a_prime},{b_prime}\n")
