# 詳細設計書

## 1. モジュール詳細設計

### 1.1 MainForm（メイン照合画面）

#### 1.1.1 InitializeServices メソッド

**IPO（Input-Process-Output）**:

| 項目 | 内容 |
|------|------|
| **Input** | なし |
| **Process** | 1. ConfigLoaderから設定値取得<br>2. SerialPortManager初期化・天秤接続<br>3. DataProviderFactory経由でEmployeeLoader初期化<br>4. CardConditionLoader初期化<br>5. MaterialConditionLoader初期化<br>6. BalanceManager初期化<br>7. LogManager初期化<br>8. ShelfStorageManager初期化<br>9. SessionStateManager初期化<br>10. CheckAndRestoreSession呼び出し |
| **Output** | 各種サービスオブジェクト初期化完了 |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `FileNotFoundException` | 設定ファイルなし → MessageBox表示、アプリ終了 |
| `InvalidOperationException` | シリアルポート接続失敗 → MessageBox表示、アプリ継続（天秤機能無効） |
| `Exception` | その他エラー → エラーログ記録、MessageBox表示、アプリ継続 |

**処理フロー**:
```
開始
  ↓
Try
  ↓
  ConfigLoader.LoadConfig()
  ↓
  SerialPortManager.OpenPort(portName)
  ↓
  DataProviderFactory.CreateEmployeeDataProvider()
  ↓
  CardConditionLoader.New()
  ↓
  MaterialConditionLoader.New()
  ↓
  BalanceManager.Initialize()
  ↓
  LogManager.New()
  ↓
  ShelfStorageManager.New()
  ↓
  SessionStateManager.New("session_state.json")
  ↓
  CheckAndRestoreSession()
  ↓
Catch 例外処理
  ↓
終了
```

---

#### 1.1.2 CheckAndRestoreSession メソッド

**IPO**:

| 項目 | 内容 |
|------|------|
| **Input** | SessionStateManager経由でsession_state.json読込 |
| **Process** | 1. SessionStateManager.LoadState()呼び出し<br>2. savedState Is Nothing判定<br>3. savedState.Stage <> 1判定<br>4. 復元確認ダイアログ表示<br>5. ユーザー選択に応じて復元 or 削除 |
| **Output** | セッション復元実行 or セッションファイル削除 |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `JsonException` | JSON破損 → SessionStateManager.DeleteState()、通常起動 |
| `Exception` | その他エラー → エラーログ記録、SessionStateManager.DeleteState() |

**処理フロー**:
```
開始
  ↓
Try
  ↓
  savedState = SessionStateManager.LoadState()
  ↓
  savedState Is Nothing?
    ├─ Yes → Return（通常起動）
    └─ No  → 次へ
  ↓
  savedState.Stage <> 1?
    ├─ Yes → SessionStateManager.DeleteState()、Return
    └─ No  → 次へ
  ↓
  MessageBox.Show("復元確認ダイアログ")
  ↓
  result = DialogResult.Yes?
    ├─ Yes → RestoreSession(savedState)
    └─ No  → SessionStateManager.DeleteState()
  ↓
Catch 例外処理
  ↓
終了
```

---

#### 1.1.3 RestoreSession メソッド

**IPO**:

| 項目 | 内容 |
|------|------|
| **Input** | savedState: SessionState |
| **Process** | 1. TextChangedイベント無効化<br>2. 従業員情報復元<br>3. イベント再有効化<br>4. カード情報復元<br>5. LAP厚復元<br>6. カード条件・部材条件復元（JSON → オブジェクト）<br>7. 投入前10mm照合結果復元<br>8. 投入後部材情報取得<br>9. メッセージ表示<br>10. _verificationStage = 1設定 |
| **Output** | 画面状態復元完了、第1段階完了状態へ遷移 |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `JsonException` | JSON変換エラー → MessageBox表示、ResetForm() |
| `Exception` | その他エラー → MessageBox表示、ResetForm() |

**処理フロー**:
```
開始
  ↓
Try
  ↓
  RemoveHandler txtEmployeeNo.TextChanged
  ↓
  txtEmployeeNo.Text = savedState.EmployeeNo
  lblEmployeeNameValue.Text = savedState.EmployeeName
  txtEmployeeNo.Enabled = False
  ↓
  AddHandler txtEmployeeNo.TextChanged
  ↓
  txtCardNo.Text = savedState.CardNo
  txtCardNo.Enabled = False
  ↓
  cmbLapThickness.SelectedItem = savedState.LapThickness
  ↓
  _currentCondition = JsonConvert.DeserializeObject(savedState.CardConditionJson)
  _currentMaterialCondition = JsonConvert.DeserializeObject(savedState.MaterialConditionJson)
  ↓
  カード情報表示
  必要枚数表示
  ↓
  lblPre10mmRemaining.Text = savedState.Pre10mmBefore
  lblPre10mmSecured.Text = savedState.Pre10mmAfter
  lblPre10mmUsed.Text = savedState.Pre10mmShortage
  lblPre10mmJudgment.Text = savedState.Pre10mmJudgment
  ↓
  判定結果の色設定（OK=緑、NG=赤）
  ↓
  DisplayPostMaterialsForSecondStage()
  ↓
  ShowMessage("照合OK。投入前10mmを...", Color.Green)
  ↓
  _verificationStage = 1
  ↓
Catch 例外処理
  ↓
終了
```

---

#### 1.1.4 PerformFirstStageVerification メソッド

**IPO**:

| 項目 | 内容 |
|------|------|
| **Input** | - _currentMaterialCondition（必要枚数）<br>- lblPre10mmRemaining.Text（照合前数） |
| **Process** | 1. 照合前数取得（文字列 → 数値変換）<br>2. BalanceManager.GetWeight()で重量取得<br>3. ConvertWeightToCount()で枚数変換<br>4. 過不足計算<br>5. 判定（OK/不足/過剰）<br>6. OK時: SaveSessionState()、第2段階へ遷移<br>7. NG時: エラーメッセージ表示、ログ記録 |
| **Output** | - 照合結果メッセージ表示<br>- セッション状態保存（OK時）<br>- ログ記録（NG時） |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `TimeoutException` | 天秤通信タイムアウト → エラーメッセージ、エラーログ |
| `InvalidOperationException` | シリアルポート未接続 → エラーメッセージ、エラーログ |
| `FormatException` | データ形式エラー → エラーメッセージ、エラーログ |
| `Exception` | その他エラー → エラーメッセージ、エラーログ |

**処理フロー**:
```
開始
  ↓
Try
  ↓
  pre10mmRequired = _currentMaterialCondition.Pre10mm
  pre10mmBefore = Integer.Parse(lblPre10mmRemaining.Text.Replace("個", ""))
  ↓
  weight = _balanceManager.GetWeight()
  pre10mmAfter = _balanceManager.ConvertWeightToCount(weight, 1.15)
  ↓
  pre10mmShortage = pre10mmRequired - pre10mmAfter
  ↓
  lblPre10mmSecured.Text = pre10mmAfter & "個"
  lblPre10mmUsed.Text = pre10mmShortage & "個"
  ↓
  pre10mmShortage = 0?
    ├─ Yes → [OK処理]
    │   lblPre10mmJudgment.Text = "OK"
    │   lblPre10mmJudgment.ForeColor = Color.Green
    │   ShowMessage("照合OK。投入前10mmを...", Color.Green)
    │   _verificationStage = 1
    │   SaveSessionState(pre10mmBefore, pre10mmAfter, pre10mmShortage)
    │   DisplayPostMaterialsForSecondStage()
    │
    └─ No  → [NG処理]
        pre10mmShortage >= 1?
          ├─ Yes → lblPre10mmJudgment.Text = "不足"
          └─ No  → lblPre10mmJudgment.Text = "過剰"
        lblPre10mmJudgment.ForeColor = Color.Red
        ShowMessage("不足/過剰: 投入前10mm...", Color.Red)
        _logManager.WriteInspectionLog(employeeNo, cardNo, condition, "NG:第1段階(不足/過剰)")
  ↓
Catch 例外処理
  ↓
終了
```

**変数定義**:

| 変数名 | 型 | 説明 |
|--------|-----|------|
| pre10mmRequired | Integer | 投入前10mm必要枚数 |
| pre10mmBefore | Integer | 投入前10mm照合前数 |
| pre10mmAfter | Integer | 投入前10mm照合後数 |
| pre10mmShortage | Integer | 投入前10mm過不足数（負=過剰） |
| weight | Double | 天秤から取得した重量（g） |

---

#### 1.1.5 PerformSecondStageVerification メソッド

**IPO**:

| 項目 | 内容 |
|------|------|
| **Input** | - _currentMaterialCondition（各部材の必要枚数）<br>- _currentCondition（エッジガード、気泡緩衝材の必要枚数） |
| **Process** | 1. 投入後1mm照合<br>2. 投入後5mm照合<br>3. 投入後10mm照合<br>4. エッジガード照合（必要な場合）<br>5. 気泡緩衝材照合（必要な場合）<br>6. 全部材OK判定<br>7. OK時: ログ記録、セッション削除<br>8. NG時: エラーメッセージ、ログ記録 |
| **Output** | - 照合結果メッセージ表示<br>- ログ記録<br>- セッション状態削除（OK時） |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `TimeoutException` | 天秤通信タイムアウト → エラーメッセージ、処理中断 |
| `Exception` | その他エラー → エラーメッセージ、エラーログ |

**処理フロー**:
```
開始
  ↓
Try
  ↓
  allOk = True
  ↓
  [投入後1mm照合]
  post1mmRequired = _currentMaterialCondition.Post1mm
  post1mmBefore = 天秤から取得
  weight = _balanceManager.GetWeight()
  post1mmAfter = ConvertWeightToCount(weight, 0.10)
  post1mmShortage = post1mmRequired - post1mmAfter
  ↓
  post1mmShortage = 0?
    ├─ Yes → lblPost1mmJudgment.Text = "OK", Color.Green
    └─ No  → lblPost1mmJudgment.Text = "不足/過剰", Color.Red
              allOk = False
              ShowMessage("不足/過剰: 投入後1mm...", Color.Red)
              Return（処理中断）
  ↓
  [投入後5mm照合] ※同様の処理
  ↓
  [投入後10mm照合] ※同様の処理
  ↓
  [エッジガード照合]（EdgeGuard > 0の場合のみ）
  ↓
  [気泡緩衝材照合]（BubbleInterference > 0の場合のみ）
  ↓
  allOk = True?
    ├─ Yes → [照合完了]
    │   ShowMessage("照合OK", Color.Green)
    │   _logManager.WriteInspectionLog(employeeNo, cardNo, condition, "OK")
    │   _sessionStateManager.DeleteState()
    │   _verificationStage = 2
    │
    └─ No  → [照合失敗]
        _logManager.WriteInspectionLog(employeeNo, cardNo, condition, "NG:第2段階")
  ↓
Catch 例外処理
  ↓
終了
```

**部材別重量定義**:

| 部材名 | 1枚あたりの重量（g） | 変数名 |
|--------|---------------------|--------|
| 投入後1mm | 0.10 | POST_1MM_WEIGHT |
| 投入後5mm | 0.50 | POST_5MM_WEIGHT |
| 投入後10mm | 1.00 | POST_10MM_WEIGHT |
| エッジガード | 0.30 | EDGE_GUARD_WEIGHT |
| 気泡緩衝材 | 0.20 | BUBBLE_WEIGHT |

---

### 1.2 BalanceManager（天秤管理）

#### 1.2.1 GetWeight メソッド

**IPO**:

| 項目 | 内容 |
|------|------|
| **Input** | なし |
| **Process** | 1. SerialPortManager.SendCommand("S\r\n")<br>2. SerialPortManager.ReadResponse()<br>3. レスポンス解析（"+0000.00 g\r\n"形式）<br>4. 数値抽出・変換 |
| **Output** | weight: Double（重量、単位: g） |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `TimeoutException` | 5秒以内にレスポンスなし → 例外スロー |
| `InvalidOperationException` | シリアルポート未接続 → 例外スロー |
| `FormatException` | レスポンス形式エラー → 例外スロー |

**処理フロー**:
```
開始
  ↓
  _serialPortManager.SendCommand("S\r\n")
  ↓
  response = _serialPortManager.ReadResponse()
  ↓
  response形式チェック（"+####.## g"）
    ├─ OK → 数値部分抽出
    └─ NG → FormatException
  ↓
  weight = Double.Parse(数値部分)
  ↓
  Return weight
  ↓
終了
```

**レスポンス解析例**:
```
入力: "+0012.35 g\r\n"
  ↓
抽出: "0012.35"
  ↓
変換: 12.35 (Double)
```

---

#### 1.2.2 ConvertWeightToCount メソッド

**IPO**:

| 項目 | 内容 |
|------|------|
| **Input** | - weight: Double（重量、g）<br>- unitWeight: Double（1枚あたりの重量、g） |
| **Process** | 1. count = weight / unitWeight<br>2. 小数点以下四捨五入<br>3. Integer型に変換 |
| **Output** | count: Integer（枚数） |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `DivideByZeroException` | unitWeight = 0 → 0を返却 |

**処理フロー**:
```
開始
  ↓
  unitWeight = 0?
    ├─ Yes → Return 0
    └─ No  → 次へ
  ↓
  count = weight / unitWeight
  ↓
  count = Math.Round(count, MidpointRounding.AwayFromZero)
  ↓
  Return CInt(count)
  ↓
終了
```

**計算例**:
```
例1: weight=11.5g, unitWeight=1.15g
  → count = 11.5 / 1.15 = 10.0
  → Return 10

例2: weight=1.2g, unitWeight=0.10g
  → count = 1.2 / 0.10 = 12.0
  → Return 12

例3: weight=5.7g, unitWeight=0.50g
  → count = 5.7 / 0.50 = 11.4 → 四捨五入 → 11
  → Return 11
```

---

### 1.3 SessionStateManager（セッション管理）

#### 1.3.1 SaveState メソッド

**IPO**:

| 項目 | 内容 |
|------|------|
| **Input** | state: SessionState |
| **Process** | 1. SessionState → JSON変換<br>2. ファイル書込（session_state.json） |
| **Output** | session_state.jsonファイル作成 |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `UnauthorizedAccessException` | 書込権限なし → エラーログ記録、例外スロー |
| `IOException` | ディスク容量不足等 → エラーログ記録、例外スロー |
| `JsonException` | JSON変換エラー → エラーログ記録、例外スロー |

**処理フロー**:
```
開始
  ↓
Try
  ↓
  json = JsonConvert.SerializeObject(state, Formatting.Indented)
  ↓
  File.WriteAllText(_filePath, json, Encoding.UTF8)
  ↓
Catch 例外処理
  ↓
終了
```

**JSON出力例**:
```json
{
  "Timestamp": "2025-11-10T14:30:00",
  "Stage": 1,
  "EmployeeNo": "123456",
  "EmployeeName": "山田太郎",
  "CardNo": "E00123",
  "LapThickness": "250μm",
  "Pre10mmBefore": 2,
  "Pre10mmAfter": 1,
  "Pre10mmShortage": 0,
  "Pre10mmJudgment": "OK",
  "CardConditionJson": "{\"CardNo\":\"E00123\",...}",
  "MaterialConditionJson": "{\"CardNo\":\"E00123\",...}"
}
```

---

#### 1.3.2 LoadState メソッド

**IPO**:

| 項目 | 内容 |
|------|------|
| **Input** | なし（_filePathから読込） |
| **Process** | 1. ファイル存在確認<br>2. JSON読込<br>3. JSON → SessionState変換<br>4. 破損チェック |
| **Output** | SessionState or Nothing |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `FileNotFoundException` | ファイルなし → Nothing返却 |
| `JsonException` | JSON破損 → ファイル削除、Nothing返却 |
| `Exception` | その他エラー → ファイル削除、Nothing返却 |

**処理フロー**:
```
開始
  ↓
  File.Exists(_filePath)?
    ├─ No  → Return Nothing
    └─ Yes → 次へ
  ↓
Try
  ↓
  json = File.ReadAllText(_filePath, Encoding.UTF8)
  ↓
  state = JsonConvert.DeserializeObject(Of SessionState)(json)
  ↓
  Return state
  ↓
Catch JsonException
  ↓
  File.Delete(_filePath)
  Return Nothing
  ↓
Catch Exception
  ↓
  File.Delete(_filePath)
  Return Nothing
  ↓
終了
```

---

### 1.4 LogManager（ログ管理）

#### 1.4.1 WriteInspectionLog メソッド

**IPO**:

| 項目 | 内容 |
|------|------|
| **Input** | - employeeNo: String<br>- cardNo: String<br>- condition: CardCondition<br>- result: String |
| **Process** | 1. 現在日時取得<br>2. ログディレクトリ存在確認・作成<br>3. ログファイル名生成（YYYYMMDD.csv）<br>4. CSV行作成<br>5. ファイル追記 |
| **Output** | logs/YYYYMMDD.csvファイルに追記 |

**例外処理**:

| 例外種類 | 処理内容 |
|---------|---------|
| `UnauthorizedAccessException` | 書込権限なし → エラーログ記録 |
| `IOException` | ディスク容量不足等 → エラーログ記録 |

**処理フロー**:
```
開始
  ↓
  timestamp = DateTime.Now
  logFileName = timestamp.ToString("yyyyMMdd") & ".csv"
  logFilePath = Path.Combine(_logDirectory, logFileName)
  ↓
  Directory.Exists(_logDirectory)?
    ├─ No  → Directory.CreateDirectory(_logDirectory)
    └─ Yes → 次へ
  ↓
  csvLine = timestamp & "," & employeeNo & "," & cardNo & "," &
            condition.ProductName & "," & condition.LotNo & "," & result
  ↓
Try
  ↓
  File.AppendAllText(logFilePath, csvLine & vbCrLf, Encoding.UTF8)
  ↓
Catch 例外処理
  ↓
終了
```

**ログ出力例**:
```csv
2025-11-10 14:30:45,123456,E00123,AAA,000001###,OK
2025-11-10 14:35:20,123456,E00124,BBB,000002###,NG:第1段階(不足)
```

---

## 2. シーケンス図

### 2.1 第1段階照合シーケンス図（DrawIO形式）

以下のシーケンス図をDrawIOで作成します:

```
[ユーザー] → [MainForm] : 照合ボタン押下
[MainForm] → [MainForm] : PerformFirstStageVerification()
[MainForm] → [BalanceManager] : GetWeight()
[BalanceManager] → [SerialPortManager] : SendCommand("S\r\n")
[SerialPortManager] → [天秤] : "S\r\n"
[天秤] → [SerialPortManager] : "+0012.35 g\r\n"
[SerialPortManager] → [BalanceManager] : response
[BalanceManager] → [BalanceManager] : 数値抽出・変換
[BalanceManager] → [MainForm] : weight (12.35)
[MainForm] → [BalanceManager] : ConvertWeightToCount(12.35, 1.15)
[BalanceManager] → [MainForm] : count (11)
[MainForm] → [MainForm] : 過不足計算
[MainForm] → [MainForm] : 判定（OK/NG）
[MainForm] → [SessionStateManager] : SaveState(state) ※OK時のみ
[SessionStateManager] → [ファイル] : session_state.json書込
[MainForm] → [MainForm] : ShowMessage("照合OK...", Green)
[MainForm] → [ユーザー] : メッセージ表示
```

---

### 2.2 セッション復元シーケンス図

```
[システム] → [MainForm] : 起動
[MainForm] → [MainForm] : InitializeServices()
[MainForm] → [SessionStateManager] : LoadState()
[SessionStateManager] → [ファイル] : session_state.json読込
[ファイル] → [SessionStateManager] : JSON文字列
[SessionStateManager] → [SessionStateManager] : JSON → SessionState
[SessionStateManager] → [MainForm] : SessionState
[MainForm] → [MainForm] : Stage = 1?
[MainForm] → [ユーザー] : 復元確認ダイアログ表示
[ユーザー] → [MainForm] : はい選択
[MainForm] → [MainForm] : RestoreSession(savedState)
[MainForm] → [MainForm] : RemoveHandler txtEmployeeNo.TextChanged
[MainForm] → [MainForm] : 従業員情報復元
[MainForm] → [MainForm] : AddHandler txtEmployeeNo.TextChanged
[MainForm] → [MainForm] : カード情報復元
[MainForm] → [MainForm] : JSON → CardCondition
[MainForm] → [MainForm] : JSON → MaterialCondition
[MainForm] → [MainForm] : 投入前10mm照合結果復元
[MainForm] → [BalanceManager] : GetWeight() ※投入後部材用
[BalanceManager] → [MainForm] : weight
[MainForm] → [MainForm] : ShowMessage("照合OK...", Green)
[MainForm] → [ユーザー] : 画面表示
```

---

## 3. データフロー図

### 3.1 照合処理データフロー

```
┌──────────────┐
│  ユーザー    │
└──────────────┘
      │
      ↓ 従業員No入力
┌──────────────┐      ┌──────────────┐
│  MainForm    │ --→ │EmployeeLoader│
└──────────────┘      └──────────────┘
      ↑                      │
      └──────────────────────┘ 従業員名
      │
      ↓ カードNo、LAP厚入力
┌──────────────┐      ┌────────────────────┐
│  MainForm    │ --→ │CardConditionLoader │
└──────────────┘      └────────────────────┘
      ↑                      │
      └──────────────────────┘ カード条件
      │
      ↓
┌──────────────┐      ┌──────────────────────────┐
│  MainForm    │ --→ │MaterialConditionLoader   │
└──────────────┘      └──────────────────────────┘
      ↑                      │
      └──────────────────────┘ 部材条件
      │
      ↓ 照合ボタン押下
┌──────────────┐      ┌──────────────┐
│  MainForm    │ --→ │BalanceManager│
└──────────────┘      └──────────────┘
      ↑                      │
      │                      ↓
      │              ┌──────────────┐
      │              │SerialPort    │
      │              │Manager       │
      │              └──────────────┘
      │                      │
      │                      ↓
      │              ┌──────────────┐
      │              │    天秤      │
      │              └──────────────┘
      │                      │
      └──────────────────────┘ 重量データ
      │
      ↓ 照合結果
┌──────────────┐      ┌──────────────────────┐
│  MainForm    │ --→ │SessionStateManager   │
└──────────────┘      └──────────────────────┘
      │                      │
      │                      ↓
      │              ┌──────────────┐
      │              │session_state │
      │              │.json         │
      │              └──────────────┘
      │
      ↓ 照合完了
┌──────────────┐      ┌──────────────┐
│  MainForm    │ --→ │ LogManager   │
└──────────────┘      └──────────────┘
                            │
                            ↓
                    ┌──────────────┐
                    │YYYYMMDD.csv  │
                    └──────────────┘
```

---

## 4. 状態遷移図

### 4.1 MainForm状態遷移

```
[初期状態]
  ├─ 従業員No入力 → [従業員認証完了]
  │
[従業員認証完了]
  ├─ カードNo入力 → [カード情報取得完了]
  │
[カード情報取得完了]
  ├─ 照合ボタン押下 → 第1段階照合実行
  │   ├─ OK → [第1段階完了]
  │   └─ NG → [カード情報取得完了]（状態維持）
  │
[第1段階完了]
  ├─ 照合ボタン押下 → 第2段階照合実行
  │   ├─ OK → [照合完了]
  │   └─ NG → [第1段階完了]（状態維持）
  │
[照合完了]
  ├─ 棚入庫ボタン押下 → [棚入庫画面表示]
  └─ キャンセルボタン押下 → [初期状態]
```

**状態変数**: `_verificationStage`
- 0: 初期状態～カード情報取得完了
- 1: 第1段階完了
- 2: 照合完了

---

## 5. エラーコード一覧

### 5.1 システムエラーコード

| エラーコード | エラー名 | 説明 | 対処方法 |
|-------------|---------|------|---------|
| ERR-001 | 設定ファイル読込エラー | App.configが見つからない | 設定ファイルを確認 |
| ERR-002 | シリアルポート接続エラー | COMポートが開けない | ポート番号、接続を確認 |
| ERR-003 | 天秤通信タイムアウト | 5秒以内にレスポンスなし | 天秤の電源、接続を確認 |
| ERR-004 | データ形式エラー | CSV形式不正 | CSVファイルを確認 |
| ERR-005 | JSON変換エラー | JSON形式不正 | session_state.jsonを削除 |
| ERR-006 | ファイル書込エラー | ディスク容量不足 | ディスク容量を確認 |
| ERR-007 | TCP通信エラー | サーバー接続失敗 | ネットワーク設定を確認 |

---

## 6. パフォーマンスチューニング指針

### 6.1 メモリ最適化

| 項目 | 最適化内容 |
|------|-----------|
| データキャッシング | カード条件、部材条件を初回読込時にメモリキャッシュ |
| リソース解放 | Using文でファイル・通信リソースを確実に解放 |
| イベント登録解除 | フォームDispose時にイベントハンドラー解除 |

### 6.2 処理速度最適化

| 項目 | 最適化内容 |
|------|-----------|
| 非同期処理 | 従業員情報取得にAsync/Await使用 |
| 遅延読込 | 棚入出庫データは画面表示時のみ読込 |
| インデックス化 | カード条件をDictionaryでキャッシュ（カードNo → CardCondition） |

---

## 改訂履歴

| 版数 | 改訂日 | 改訂者 | 改訂内容 |
|------|--------|--------|----------|
| 1.0 | 2025-11-10 | TakumaYamaguchi | 初版作成 |

