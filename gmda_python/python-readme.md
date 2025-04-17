# Gardony Map Drawing Analyzer (GMDA) - Python Implementation

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

## Overview

The Gardony Map Drawing Analyzer (GMDA) is a tool for quantitatively analyzing sketch maps in spatial cognition research. GMDA helps researchers quantify the accuracy and characteristics of hand-drawn sketch maps compared to reference maps. Originally developed as a C# application, this Python implementation provides the same basic analytical capabilities in a more extensible, cross-platform format.

## Features

- **Canonical Spatial Analysis**: Evaluates North/South and East/West relationships between landmarks
- **Metric Analysis**: Calculates distances and angles between landmarks
- **Bidimensional Regression (BDR)**: Provides statistical measures of map similarity
- **Individual Landmark Analysis**: Evaluates canonical measures of each landmark's placement
- **Missing Landmark Support**: Properly handles landmarks that were omitted in sketch maps
- **Comprehensive Output**: Generates both summary and detailed output files

## Installation

### Requirements

- Python 3.6 or higher
- NumPy
- Pandas
- Matplotlib (for visualization examples)

### Installation Steps

1. Clone this repository:
```bash
git clone https://github.com/agardony/GMDA.git
cd gmda-python
```

2. Install dependencies:
```bash
pip install numpy pandas matplotlib
```

## Usage

### Basic Usage

```python
import gmda

# Analyze maps
gmda_measures, bdr_params = gmda.analyze_maps(
    reference_file='path/to/reference_map.csv',
    sketch_file='path/to/sketch_map.csv',
    output_prefix='analysis_results'
)
```

### Input File Format

Both reference and sketch map files should be CSV files with the following columns:

**Reference Map File:**
- `landmark_name`: Name of the landmark
- `x_coordinate`: X-coordinate in arbitrary units
- `y_coordinate`: Y-coordinate in arbitrary units

**Sketch Map File:**
- `landmark_name`: Name of the landmark (must match reference map)
- `x_coordinate`: X-coordinate in arbitrary units
- `y_coordinate`: Y-coordinate in arbitrary units
- `is_missing`: Optional column - mark 'y' if the landmark was omitted in the sketch map

** IMPORTANT: Both the reference map and sketch map coordinates must be in the Cartesian coordinate system **

### Example

An example script is provided in `example_gmda.py`, which demonstrates:
1. Creating sample reference and sketch map data
2. Visualizing the maps
3. Running the GMDA analysis
4. Interpreting the results

To run the example:
```bash
python example_gmda.py
```

## Understanding the Results

GMDA provides several metrics to evaluate sketch map accuracy:

### Canonical Measures

- **Canonical Organization**: Percentage of correctly oriented landmark pairs
- **Square Root of Canonical Organization**: Transformation for better metric correlation with other measures
- **Canonical Accuracy**: Percentage of correctly oriented landmark pairs, excluding missing landmarks

### Distance and Angle Measures

- **Scaling Bias**: Average difference between expected and observed distance ratios
- **Rotational Bias**: Average angular difference in landmark configurations
- **Distance Accuracy**: Accuracy in relative distances between landmarks (0-1 scale)
- **Angle Accuracy**: Accuracy in angles between landmarks (0-1 scale)

### Bidimensional Regression Parameters

- **r**: Correlation coefficient indicating overall map accuracy
- **scale**: Scaling factor between reference and sketch map
- **theta**: Rotation angle between reference and sketch map
- **DI**: Distortion index measuring overall configurational error

### Output Files

GMDA generates two output files:

1. **Summary File** (`*_summary.csv`): Contains the following sections:
   - Configural GMDA measures
   - Bidimensional regression parameters
   - Individual landmark measures

2. **Raw Data File** (`*_raw.csv`): Contains detailed analysis data for each landmark pair:
   - Canonical judgments (N/S, E/W)
   - Distance and angle measurements
   - Difference metrics between reference and sketch maps
   - Bidimensional regression predicted coordinates

## Extending GMDA

You can extend GMDA with additional analyses or visualizations:

```python
import gmda
import matplotlib.pyplot as plt
import pandas as pd

# Run the analysis
ref_file = 'reference_map.csv'
sketch_file = 'sketch_map.csv'
gmda_measures, bdr_params = gmda.analyze_maps(ref_file, sketch_file)

# Create custom visualizations
ref_data = pd.read_csv(ref_file)
sketch_data = pd.read_csv(sketch_file)

# Plot reference vs. sketch map
plt.figure(figsize=(10, 5))
plt.subplot(121)
plt.scatter(ref_data['x_coordinate'], ref_data['y_coordinate'])
plt.title('Reference Map')

plt.subplot(122)
plt.scatter(sketch_data['x_coordinate'], sketch_data['y_coordinate'])
plt.title('Sketch Map')
plt.show()
```

## Function Documentation

The GMDA module provides the following main functions:

- `analyze_maps(reference_file, sketch_file, output_prefix=None)`: Main analysis function
- `read_input_files(reference_file, sketch_file)`: Reads coordinate files
- `compute_BDR(ref_data, sketch_data, dependent_is_sketch=True)`: Calculates bidimensional regression
- `calculate_gmda_measures(...)`: Calculates GMDA metrics
- `generate_summary_file(...)`: Creates summary output file
- `generate_raw_file(...)`: Creates detailed output file

For complete documentation of all functions, refer to the docstrings in the code.

## Citation

When using GMDA in research publications, please cite:

Gardony, A.L., Taylor, H.A., & Brunyé, T.T. (2016). Gardony Map Drawing Analyzer: Software for quantitative analysis of sketch maps. *Behavior Research Methods, 48*(1), 151-177.

## License

This program is distributed under the terms of the GNU General Public License v3.

## Acknowledgments

The Python implementation is based on the original C# GMDA software developed by Aaron Gardony.
