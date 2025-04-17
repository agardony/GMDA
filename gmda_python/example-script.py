"""
Example script demonstrating how to use the GMDA Python implementation.

This script shows how to:
1. Create sample reference and sketch maps
2. Analyze the maps using GMDA
3. Interpret the results

Author: Your Name
Date: April 2, 2025
"""

import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import os
from pathlib import Path
import gmda  # Import the GMDA module

def create_example_data():
    """
    Create sample reference and sketch map coordinate files for demonstration.
    
    Returns
    -------
    ref_file : str
        Path to reference map coordinates file
    sketch_file : str
        Path to sketch map coordinates file
    """
    # Create a directory for example data
    example_dir = Path('example_data')
    example_dir.mkdir(exist_ok=True)
    
    # Create reference map data (perfect grid pattern)
    landmarks = [
        "Town Hall", "Library", "Park", "School", "Church", 
        "Market", "Hospital", "Bank", "Mall"
    ]
    
    # Create a 3x3 grid
    x_coords = [0, 0, 0, 50, 50, 50, 100, 100, 100]
    y_coords = [0, 50, 100, 0, 50, 100, 0, 50, 100]
    
    ref_data = pd.DataFrame({
        'landmark_name': landmarks,
        'x_coordinate': x_coords,
        'y_coordinate': y_coords
    })
    
    # Create sketch map data (distorted version of reference map)
    # Apply some random distortion
    np.random.seed(8675309)  # For reproducibility
    
    # Apply scaling, rotation, and random noise
    scale = 0.8
    rotation_rad = np.radians(15)  # 15-degree rotation
    
    sketch_x = []
    sketch_y = []
    
    for x, y in zip(x_coords, y_coords):
        # Apply transformation matrix (scale and rotation)
        x_rot = scale * (x * np.cos(rotation_rad) - y * np.sin(rotation_rad))
        y_rot = scale * (x * np.sin(rotation_rad) + y * np.cos(rotation_rad))
        
        # Add random noise
        x_sketch = x_rot + np.random.normal(0, 3)
        y_sketch = y_rot + np.random.normal(0, 3)
        
        sketch_x.append(x_sketch)
        sketch_y.append(y_sketch)
    
    # Mark one landmark as missing to demonstrate that feature
    is_missing = ['', '', '', '', '', 'y', '', '', '']  # Mark "Market" as missing
    
    sketch_data = pd.DataFrame({
        'landmark_name': landmarks,
        'x_coordinate': sketch_x,
        'y_coordinate': sketch_y,
        'is_missing': is_missing
    })
    
    # Save the data to CSV files
    ref_file = example_dir / 'reference_map.csv'
    sketch_file = example_dir / 'sketch_map.csv'
    
    ref_data.to_csv(ref_file, index=False)
    sketch_data.to_csv(sketch_file, index=False)
    
    print(f"Example data files created:")
    print(f"- Reference map: {ref_file}")
    print(f"- Sketch map: {sketch_file}")
    
    return str(ref_file), str(sketch_file)

def visualize_maps(ref_file, sketch_file):
    """
    Visualize reference and sketch map data.
    
    Parameters
    ----------
    ref_file : str
        Path to reference map coordinates file
    sketch_file : str
        Path to sketch map coordinates file
    """
    ref_data = pd.read_csv(ref_file)
    sketch_data = pd.read_csv(sketch_file)
    
    # Create a figure with two subplots
    fig, (ax1, ax2) = plt.subplots(1, 2, figsize=(12, 6))
    
    # Plot reference map
    ax1.scatter(ref_data['x_coordinate'], ref_data['y_coordinate'], color='blue', s=100)
    for i, landmark in enumerate(ref_data['landmark_name']):
        ax1.annotate(landmark, 
                    (ref_data['x_coordinate'][i], ref_data['y_coordinate'][i]),
                    xytext=(5, 5), textcoords='offset points')
    ax1.set_title('Reference Map')
    ax1.set_xlabel('X Coordinate')
    ax1.set_ylabel('Y Coordinate')
    ax1.grid(True)
    
    # Plot sketch map
    for i, row in sketch_data.iterrows():
        is_missing = row['is_missing'] == 'y'
        color = 'red' if is_missing else 'green'
        if not is_missing:
            ax2.scatter(row['x_coordinate'], row['y_coordinate'], color=color, s=100)
            ax2.annotate(row['landmark_name'], 
                        (row['x_coordinate'], row['y_coordinate']),
                        xytext=(5, 5), textcoords='offset points')
        else:
            ax2.annotate(f"{row['landmark_name']} (missing)", 
                        (10, 10 + i*10), 
                        xytext=(5, 5), textcoords='offset points', color='red')
    
    ax2.set_title('Sketch Map')
    ax2.set_xlabel('X Coordinate')
    ax2.set_ylabel('Y Coordinate')
    ax2.grid(True)
    
    plt.tight_layout()
    
    # Save the figure
    output_dir = Path('example_outputs')
    output_dir.mkdir(exist_ok=True)
    plt.savefig(output_dir / 'maps_visualization.png', dpi=300)
    
    print("Maps visualization saved as 'example_outputs/maps_visualization.png'")
    plt.show()

def display_results_directly(gmda_measures, bdr_params):
    """
    Display the GMDA analysis results directly from the returned data.
    
    Parameters
    ----------
    gmda_measures : dict
        GMDA measures returned by analyze_maps
    bdr_params : dict
        Bidimensional regression parameters returned by analyze_maps
    """
    # Display configural GMDA measures
    print("\n=== CONFIGURAL GMDA MEASURES ===")
    if gmda_measures:
        for measure, value in gmda_measures.items():
            print(f"{measure}: {value:.2f}")
    else:
        print("No GMDA measures available")
    
    # Display BDR parameters
    print("\n=== BDR PARAMETERS ===")
    if bdr_params:
        for param, value in bdr_params.items():
            print(f"{param}: {value:.2f}")
    else:
        print("No BDR parameters available")

def interpret_results():
    """
    Provide interpretation of the GMDA analysis results.
    """
    print("\n=== INTERPRETATION GUIDE ===")
    print("1. Canonical Organization: Percentage of correctly oriented landmark pairs")
    print("   - Higher values indicate better preservation of spatial relationships")
    print("   - SQRT transformation often provides more linear relationship with other metrics")
    
    print("\n2. Canonical Accuracy: Same as above but excludes missing landmarks")
    print("   - Use this metric when comparing maps with different missing landmarks")
    
    print("\n3. Scaling Bias: Indicates if map is scaled up (>0) or down (<0)")
    print("   - Values close to 0 indicate minimal scaling distortion")
    
    print("\n4. Rotational Bias: Average angular difference in degrees")
    print("   - Values close to 0 indicate minimal rotational distortion")
    
    print("\n5. Distance and Angle Accuracy: Range from 0-1")
    print("   - Higher values indicate better preservation of relative distances and angles")
    
    print("\n6. BDR Parameters:")
    print("   - r: Correlation coefficient (0-1); higher values indicate better correspondence")
    print("   - scale: Overall scaling factor between maps")
    print("   - theta: Overall rotation angle in degrees")
    print("   - DI: Distortion index; lower values indicate less distortion")

def main():
    """
    Main function to execute the example.
    """
    print("=== GMDA EXAMPLE SCRIPT ===")
    
    # Step 1: Create example data
    ref_file, sketch_file = create_example_data()
    
    # Step 2: Visualize the maps
    visualize_maps(ref_file, sketch_file)
    
    # Step 3: Create output directory if it doesn't exist
    output_dir = Path('example_outputs')
    output_dir.mkdir(exist_ok=True)
    
    # Step 4: Analyze the maps using GMDA
    print("\nAnalyzing maps with GMDA...")
    output_prefix = str(output_dir / "example_analysis")
    gmda_measures, bdr_params = gmda.analyze_maps(ref_file, sketch_file, output_prefix)
    
    # Step 5: Display results directly from the returned data
    display_results_directly(gmda_measures, bdr_params)
    
    # Step 6: Provide interpretation guide
    interpret_results()
    
    print("\nComplete! Results have been saved to:")
    print(f"- Summary file: {output_prefix}_summary.csv")
    print(f"- Raw data file: {output_prefix}_raw.csv")

if __name__ == "__main__":
    main()