# BalanceInspection UI and Feature Modifications

## Overview
This document describes the UI and feature modifications made to the BalanceInspection application.

## Changes Made

### 1. UI Layout Changes
- **Card Information Panel**: Moved down by 50 pixels (from Y:210 to Y:260)
- **Material Conditions Panel**: Moved down by 50 pixels (from Y:430 to Y:480)
- **New LAP Thickness Dropdown**: Added between CardNo and the panels (at Y:205)

### 2. New LAP Thickness Feature
- Added a dropdown control (`cmbLapThickness`) for selecting LAP thickness
- LAP thickness options are configurable via `AppConfig.LapThicknessList`
- Default values: 200μm, 250μm, 290μm, 300μm, 350μm

### 3. Workflow Changes
**Old Workflow:**
1. Enter Employee No → Verify
2. Enter Card No → Automatically fetch data and display material conditions

**New Workflow:**
1. Enter Employee No → Verify
2. Enter Card No → Display card information (product name, quantity, location)
3. Select LAP Thickness → Fetch material conditions and balance readings
4. Re-selecting LAP Thickness re-fetches conditions and balance readings

### 4. Material Conditions Management
- Material conditions are now stored in a separate CSV file (`material_conditions.csv`)
- CSV structure: `枚数,LAP厚,投入前10mm,投入後1mm,投入後5mm,投入後10mm`
- Material conditions are filtered by matching both quantity (from card info) and LAP thickness
- Edge guard and bubble interference information still comes from card data

### 5. New Models and Services
- **MaterialCondition Model**: Holds material condition data (quantity, LAP thickness, cushion materials)
- **MaterialConditionLoader Service**: Loads and manages material conditions from CSV
- **AppConfig Extension**: Added `MaterialConditionCsvPath` and `LapThicknessList` properties

### 6. Data Retrieval Trigger
- Changed from "after Card No entry" to "after LAP thickness selection"
- Balance readings are now taken when LAP thickness is selected
- Re-selecting LAP thickness triggers new balance readings

## Configuration

### AppConfig.vb
```vb
Public Property MaterialConditionCsvPath As String = "material_conditions.csv"
Public Property LapThicknessList As List(Of String) = New List(Of String)(New String() {"200μm", "250μm", "290μm", "300μm", "350μm"})
```

### material_conditions.csv Format
```csv
枚数,LAP厚,投入前10mm,投入後1mm,投入後5mm,投入後10mm
25,200μm,1,2,0,0
25,250μm,1,1,2,1
```

## Files Modified
1. `MainForm.vb` - Added LAP thickness event handler, updated workflow logic
2. `MainForm.Designer.vb` - Added LAP thickness dropdown control, repositioned panels
3. `Models/AppConfig.vb` - Added material condition CSV path and LAP thickness list
4. `Models/MaterialCondition.vb` - New model for material conditions
5. `Services/MaterialConditionLoader.vb` - New service for loading material conditions
6. `BalanceInspection.vbproj` - Added new files to project

## Testing Checklist
- [ ] Employee No entry works correctly
- [ ] Card No entry displays card information
- [ ] LAP thickness dropdown is enabled after valid card entry
- [ ] Material conditions are displayed after LAP thickness selection
- [ ] Balance readings are taken after LAP thickness selection
- [ ] Re-selecting LAP thickness re-fetches data
- [ ] Edge guard and bubble interference display correctly from card data
- [ ] Verification workflow completes successfully
- [ ] Error handling works for missing material conditions
