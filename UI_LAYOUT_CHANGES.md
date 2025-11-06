# UI Layout Changes

## Before Changes

```
+-----------------------------------------------+
| Message Area (Y: 58)                          |
+-----------------------------------------------+
| 従業員No: [______] 氏名                       |  (Y: 117)
| カードNo: [______]                            |  (Y: 161)
+-----------------------------------------------+
| カード情報 Panel (Y: 210)                     |
| - カードNo                                     |
| - 品名                                         |
| - 枚数                                         |
| - 所在                                         |
+-----------------------------------------------+
| 使用部材条件 Panel (Y: 430)                   |
| - 投入前10mm, 投入後1mm, etc.                 |
+-----------------------------------------------+
```

## After Changes

```
+-----------------------------------------------+
| Message Area (Y: 58)                          |
+-----------------------------------------------+
| 従業員No: [______] 氏名                       |  (Y: 117)
| カードNo: [______]                            |  (Y: 161)
| LAP厚:    [▼___▼]  ← NEW!                    |  (Y: 205)
+-----------------------------------------------+
| カード情報 Panel (Y: 260) ← Moved down 50px   |
| - カードNo                                     |
| - 品名                                         |
| - 枚数                                         |
| - 所在                                         |
+-----------------------------------------------+
| 使用部材条件 Panel (Y: 480) ← Moved down 50px |
| - 投入前10mm, 投入後1mm, etc.                 |
+-----------------------------------------------+
```

## Control Details

### New LAP Thickness ComboBox
- **Name**: cmbLapThickness
- **Type**: ComboBox (DropDownList style)
- **Location**: (186, 205)
- **Size**: (213, 32)
- **Font**: MS UI Gothic, 12pt
- **TabIndex**: 4
- **Initially Disabled**: True (enabled after Card No entry)

### New LAP Thickness Label
- **Name**: lblLapThickness
- **Text**: "LAP厚:"
- **Location**: (30, 208)
- **Font**: MS UI Gothic, 12pt

## Interaction Flow

### State 1: Initial
```
従業員No: [ENABLED ]  氏名
カードNo: [DISABLED]
LAP厚:    [DISABLED]
```

### State 2: After Employee No
```
従業員No: [DISABLED]  山田太郎
カードNo: [ENABLED ]
LAP厚:    [DISABLED]
```

### State 3: After Card No (6 digits)
```
従業員No: [DISABLED]  山田太郎
カードNo: [DISABLED]  e00123
LAP厚:    [ENABLED ]   ← Can select now!

Card Info Panel: Shows product info
Material Conditions Panel: Empty (waiting for LAP selection)
```

### State 4: After LAP Selection
```
従業員No: [DISABLED]  山田太郎
カードNo: [DISABLED]  e00123
LAP厚:    [ENABLED ]   250μm

Card Info Panel: Shows product info
Material Conditions Panel: Shows conditions
Balance readings: Displayed
照合 Button: [ENABLED]
```

## Visual Spacing

```
従業員No Label (Y: 120)
├─ 44px gap
└─ カードNo Label (Y: 164) ← Original position adjusted

カードNo Label (Y: 164)
├─ 44px gap  
└─ LAP厚 Label (Y: 208) ← NEW!

LAP厚 Combobox (Y: 205)
├─ 55px gap
└─ Card Info Panel (Y: 260)

Card Info Panel (Y: 260)
├─ 220px gap (panel height + spacing)
└─ Material Conditions Panel (Y: 480)
```

## Color Scheme
All controls maintain the existing application color scheme:
- Background: Default (SystemColors.Control)
- Text: Default (SystemColors.ControlText)
- Disabled: Gray
- Panels: White background with gray border

## Accessibility
- TabIndex properly ordered: 1 (Employee), 3 (Card), 4 (LAP), 5 (Verify), 6 (Cancel)
- Keyboard navigation supported
- Clear visual feedback for enabled/disabled states
- Focus automatically moves to next control when appropriate
