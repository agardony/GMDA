# Gardony Map Drawing Analyzer (GMDA)

The Gardony Map Drawing Analyzer (GMDA) is a tool for analyzing sketch maps in spatial cognition research. 

Click [here](https://www.aarongardony.com/tools/map-drawing-analyzer) to download the compiled Windows GMDA application.

Click [here](https://link.springer.com/article/10.3758/s13428-014-0556-x) for the GMDA paper published in Behavior Research Methods.

## Citation

When using GMDA in research publications, please cite:

Gardony, A.L., Taylor, H.A., & Brunyé, T.T. (2016). Gardony Map Drawing Analyzer: Software for quantitative analysis of sketch maps. *Behavior Research Methods, 48*(1), 151-177.

## License

This program is distributed under the terms of the GNU General Public License v3.

## Acknowledgments

GMDA was conceived and developed by Aaron Gardony. The Python implementation was developed by Aaron Gardony with the assistance of Anthropic Claude 3.7 Sonnet.

## Overview

This repository provides source code for:
1. The original C# implementation (Windows-Only GUI App)
2. A pyhon re-implementation of GMDA's basic analysis functionality (Command-line only, advanced analysis unsupported)

GMDA analyzes two sets of spatial coordinates:
1. **Reference Map**: The "ground truth" coordinates of landmarks
2. **Sketch Map**: User-drawn coordinates of those same landmarks

The analysis generates both configural (whole-map) and per-landmark measures of spatial accuracy.

## Key Features

- Processing of reference and sketch map coordinates
- Handling of missing landmarks
- Calculation of GMDA measures:
  - Canonical Organization
  - Canonical Accuracy
  - Scaling Bias
  - Rotational Bias
  - Distance Accuracy
  - Angle Accuracy
- Bidimensional regression analysis
- Generation of detailed output files

## Installation

```bash
git https://github.com/agardony/GMDA.git
```

For Windows users, you can download the GMDA executable [here](https://www.aarongardony.com/tools/map-drawing-analyzer).

### Windows-Specific Instructions

To alter GMDA source code and run on your PC you must have Visual Studio installed on your computer. You must also have version 4 of Microsoft.NET installed on your computer. On my machine .NET is installed at: C:\Windows\Microsoft.NET\Framework\v4.0.30319

If your version differs you may need to alter the build events in order to sucessfully build the application in Visual Studio.

To do this go to Project --> MapDrawingAnalyzer Properties (or press ALT-F7). Then go to build events. In the Post-build event command line section find this line:

"Map Drawing Analyzer\Ilmerge\ILMerge.exe" "Map Drawing Analyzer\Ilmerge\GMDA.exe" "Map Drawing Analyzer\Ilmerge\MessageBoxManager.dll" "Map Drawing Analyzer\Ilmerge\AppLimit.NetSparkle.dll" /targetplatform:v4,"C:\Windows\Microsoft.NET\Framework\v4.0.30319" /t:winexe /out:"Map Drawing Analyzer\Ilmerge\dest\GMDA.exe"

The important section is:

/targetplatform:v4,"C:\Windows\Microsoft.NET\Framework\v4.0.30319"

Change to the directory in quotes to point to where your .NET installation is located.

### Python-Specific Instructions
See python-readme.md in the gmda-python folder.