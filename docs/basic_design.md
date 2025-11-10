# 概要設計書（基本設計書）

## 1. システム概要

### 1.1 システム名
**部材照合システム（Balance Inspection System）**

### 1.2 システム構成

```
┌─────────────────────────────────────────────────────────────┐
│                     クライアントPC                            │
│  ┌───────────────────────────────────────────────────────┐  │
│  │         部材照合アプリケーション (.NET 4.7.1)           │  │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  │  │
│  │  │ MainForm    │  │ Shelf       │  │ Shelf       │  │  │
│  │  │ (照合画面)  │  │ StorageForm │  │ RetrievalForm│ │  │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  │  │
│  │         ↓                 ↓                 ↓         │  │
│  │  ┌───────────────────────────────────────────────┐  │  │
│  │  │              Services Layer                   │  │  │
│  │  │  - BalanceManager (天秤制御)                  │  │  │
│  │  │  - CardConditionLoader (カード条件取得)       │  │  │
│  │  │  - MaterialConditionLoader (部材条件取得)     │  │  │
│  │  │  - EmployeeLoader (従業員情報取得)            │  │  │
│  │  │  - LogManager (ログ記録)                      │  │  │
│  │  │  - SessionStateManager (セッション管理)       │  │  │
│  │  │  - ShelfStorageManager (棚管理)               │  │  │
│  │  │  - SerialPortManager (シリアル通信)           │  │  │
│  │  │  - TcpCommunicationManager (TCP通信)          │  │  │
│  │  └───────────────────────────────────────────────┘  │  │
│  │         ↓                                             │  │
│  │  ┌───────────────────────────────────────────────┐  │  │
│  │  │              Models Layer                     │  │  │
│  │  │  - CardCondition (カード条件)                 │  │  │
│  │  │  - MaterialCondition (部材条件)               │  │  │
│  │  │  - BalanceConfig (天秤設定)                   │  │  │
│  │  │  - SessionState (セッション状態)              │  │  │
│  │  │  - ShelfStorage (棚入庫情報)                  │  │  │
│  │  └───────────────────────────────────────────────┘  │  │
│  └───────────────────────────────────────────────────────┘  │
│                                                             │
│  ┌──────────────┐   ┌──────────────┐   ┌──────────────┐  │
│  │ Serial Port  │   │ TCP Socket   │   │ Local Files  │  │
│  │ (COM1-20)    │   │ (5001, 5002) │   │ (CSV, JSON)  │  │
│  └──────────────┘   └──────────────┘   └──────────────┘  │
└─────────────────────────────────────────────────────────────┘
         │                      │                      │
         ↓                      ↓                      ↓
   ┌──────────┐         ┌──────────────┐      ┌──────────────┐
   │  天秤    │         │ 外部サーバー │      │ローカルストレージ│
   │(RS-232C) │         │(従業員/カード)│      │(logs, config)│
   └──────────┘         └──────────────┘      └──────────────┘
```

---

## 2. アーキテクチャ設計

### 2.1 レイヤー構成

#### 2.1.1 プレゼンテーション層（Presentation Layer）
- **責務**: ユーザーインターフェース、ユーザー入力の受付、画面表示制御
- **構成要素**:
  - `MainForm`: メイン照合画面
  - `ShelfStorageForm`: 棚入庫画面
  - `ShelfRetrievalForm`: 棚出庫画面
- **技術**: Windows Forms (.NET Framework 4.7.1)

#### 2.1.2 ビジネスロジック層（Business Logic Layer）
- **責務**: ビジネスルールの実装、データ検証、外部システム連携
- **構成要素**:
  - `BalanceManager`: 天秤制御、重量→枚数変換
  - `CardConditionLoader`: カード条件取得
  - `MaterialConditionLoader`: 部材条件取得
  - `EmployeeLoader`: 従業員情報取得
  - `LogManager`: ログ記録
  - `SessionStateManager`: セッション状態管理
  - `ShelfStorageManager`: 棚入出庫管理
  - `SerialPortManager`: シリアルポート通信
  - `TcpCommunicationManager`: TCP/IP通信
  - `DataProviderFactory`: データプロバイダー生成

#### 2.1.3 データモデル層（Data Model Layer）
- **責務**: データ構造の定義、ビジネスデータの保持
- **構成要素**:
  - `CardCondition`: カード条件モデル
  - `MaterialCondition`: 部材条件モデル
  - `BalanceConfig`: 天秤設定モデル
  - `SessionState`: セッション状態モデル
  - `ShelfStorage`: 棚入庫情報モデル
  - `AppConfig`: アプリ設定モデル

### 2.2 デザインパターン

#### 2.2.1 Factory Pattern（データプロバイダー生成）
```vb
DataProviderFactory
├─ CreateEmployeeDataProvider() → IEmployeeDataProvider
│  ├─ SocketEmployeeDataProvider (TCP通信)
│  └─ CsvEmployeeDataProvider (CSVファイル)
└─ CreateCardDataProvider() → ICardDataProvider
   ├─ SocketCardDataProvider (TCP通信)
   └─ CsvCardDataProvider (CSVファイル)
```

#### 2.2.2 Strategy Pattern（データ取得戦略）
- インターフェース: `IEmployeeDataProvider`, `ICardDataProvider`
- 具象クラス: ソケット通信版、CSVファイル版
- 利点: データソースの切り替えが容易

#### 2.2.3 Singleton Pattern（設定管理）
- `ConfigLoader`: アプリケーション設定を一元管理
- 利点: 設定の一貫性保証、メモリ効率

---

## 3. モジュール設計

### 3.1 画面モジュール

#### 3.1.1 MainForm（メイン照合画面）

**目的**: 部材照合のメイン処理を実行

**主要機能**:
1. 従業員認証
2. カード情報取得・表示
3. 第1段階照合（投入前10mm）
4. 第2段階照合（投入後部材）
5. セッション状態管理

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `InitializeServices()` | サービス初期化 | なし | なし |
| `CheckAndRestoreSession()` | セッション復元確認 | なし | なし |
| `RestoreSession(savedState)` | セッション状態復元 | SessionState | なし |
| `ProcessCardNoInput()` | カードNo入力処理 | なし | なし |
| `PerformFirstStageVerification()` | 第1段階照合実行 | なし | なし |
| `PerformSecondStageVerification()` | 第2段階照合実行 | なし | なし |
| `SaveSessionState(pre10mmBefore, pre10mmAfter, pre10mmShortage)` | セッション保存 | Integer × 3 | なし |
| `ResetForm()` | フォームリセット | なし | なし |

**状態遷移**:
- `_verificationStage`: 0=未開始, 1=第1段階完了, 2=照合完了

#### 3.1.2 ShelfStorageForm（棚入庫画面）

**目的**: 照合完了後の棚入庫情報登録

**主要機能**:
1. カード情報表示（読取専用）
2. 棚番号入力
3. 入庫日時自動設定
4. 入庫情報CSV保存

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `SetCardInfo(condition)` | カード情報設定 | CardCondition | なし |
| `btnStorage_Click()` | 入庫実行 | sender, e | なし |

#### 3.1.3 ShelfRetrievalForm（棚出庫画面）

**目的**: 棚からの出庫情報検索・登録

**主要機能**:
1. 検索条件入力（カードNo、品名、ロットNo、工程）
2. 入庫情報検索・表示
3. 出庫枚数入力
4. 出庫情報記録

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `btnSearch_Click()` | 検索実行 | sender, e | なし |
| `btnRetrieve_Click()` | 出庫実行 | sender, e | なし |
| `LoadStorageData()` | 入庫データ読込 | なし | List(Of ShelfStorage) |

---

### 3.2 ビジネスロジックモジュール

#### 3.2.1 BalanceManager（天秤管理）

**目的**: 天秤との通信、重量データ取得、枚数変換

**主要機能**:
1. 天秤との接続管理
2. 重量データ取得（シリアル通信）
3. ゼロリセット
4. 重量→枚数変換（部材ごとの重量設定に基づく）

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `Initialize()` | 天秤初期化 | なし | なし |
| `PerformInitialReading()` | 初回計測 | なし | なし |
| `GetWeight()` | 重量取得 | なし | Double |
| `ConvertWeightToCount(weight, unitWeight)` | 重量→枚数変換 | Double, Double | Integer |
| `PerformZeroReset()` | ゼロリセット | なし | なし |
| `Disconnect()` | 接続切断 | なし | なし |

**部材別重量設定**:
| 部材名 | 1枚あたりの重量 |
|--------|----------------|
| 投入前10mm | 1.15g |
| 投入後1mm | 0.10g |
| 投入後5mm | 0.50g |
| 投入後10mm | 1.00g |
| エッジガード | 0.30g |
| 気泡緩衝材 | 0.20g |

#### 3.2.2 CardConditionLoader（カード条件読込）

**目的**: カード条件マスタからデータ取得

**主要機能**:
1. TCP/IP通信またはCSVファイルからカード条件取得
2. カードNo検証
3. データキャッシング

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `GetCondition(cardNo)` | カード条件取得 | String | CardCondition |
| `LoadFromCsv()` | CSVから全件読込 | なし | List(Of CardCondition) |

#### 3.2.3 MaterialConditionLoader（部材条件読込）

**目的**: LAP厚に応じた部材条件取得

**主要機能**:
1. CSVファイルから部材条件取得
2. カードNo + LAP厚で条件特定

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `GetCondition(cardNo, lapThickness)` | 部材条件取得 | String, String | MaterialCondition |
| `LoadFromCsv()` | CSVから全件読込 | なし | List(Of MaterialCondition) |

#### 3.2.4 SessionStateManager（セッション管理）

**目的**: セッション状態の保存・復元・削除

**主要機能**:
1. セッション状態のJSON形式保存
2. 起動時の自動復元確認
3. 照合完了/キャンセル時の自動削除

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `SaveState(state)` | 状態保存 | SessionState | なし |
| `LoadState()` | 状態読込 | なし | SessionState |
| `DeleteState()` | 状態削除 | なし | なし |
| `HasState()` | 状態存在確認 | なし | Boolean |

**保存ファイル**: `session_state.json`（実行ディレクトリ）

#### 3.2.5 LogManager（ログ管理）

**目的**: 照合結果ログ、エラーログの記録

**主要機能**:
1. 日次照合ログファイル作成
2. CSV形式でログ記録
3. エラーログ記録

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `WriteInspectionLog(employeeNo, cardNo, condition, result)` | 照合ログ記録 | String × 4 | なし |
| `WriteErrorLog(message)` | エラーログ記録 | String | なし |

**ログファイル形式**:
- 照合ログ: `logs/YYYYMMDD.csv`
- エラーログ: `logs/error/YYYYMMDD_error.log`

---

### 3.3 通信モジュール

#### 3.3.1 SerialPortManager（シリアル通信）

**目的**: 天秤とのシリアル通信制御

**通信仕様**:
- **ボーレート**: 9600bps
- **データビット**: 8bit
- **パリティ**: なし
- **ストップビット**: 1bit
- **フロー制御**: なし

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `OpenPort(portName)` | ポート開放 | String | Boolean |
| `ClosePort()` | ポート閉鎖 | なし | なし |
| `SendCommand(command)` | コマンド送信 | String | なし |
| `ReadResponse()` | レスポンス読取 | なし | String |

**コマンド仕様**:
- 重量取得: `"S\r\n"`
- ゼロリセット: `"Z\r\n"`
- レスポンス形式: `"+0000.00 g\r\n"`

#### 3.3.2 TcpCommunicationManager（TCP通信）

**目的**: 外部サーバーとのTCP/IP通信

**通信仕様**:
- **プロトコル**: TCP/IP
- **エンコーディング**: UTF-8
- **タイムアウト**: 5秒

**主要メソッド**:
| メソッド名 | 概要 | 引数 | 戻り値 |
|-----------|------|------|--------|
| `Connect(ipAddress, port)` | サーバー接続 | String, Integer | Boolean |
| `Disconnect()` | 切断 | なし | なし |
| `SendRequest(request)` | リクエスト送信 | String | なし |
| `ReceiveResponse()` | レスポンス受信 | なし | String |

---

## 4. データ設計

### 4.1 データモデル

#### 4.1.1 CardCondition（カード条件）

```vb
Public Class CardCondition
    Public Property CardNo As String           ' カードNo (PK)
    Public Property ProductName As String      ' 品名
    Public Property LotNo As String            ' ロットNo
    Public Property Quantity As Integer        ' 枚数
    Public Property Location As String         ' 工程
    Public Property EdgeGuard As Integer       ' エッジガード必要枚数（0=不要）
    Public Property BubbleInterference As Integer  ' 気泡緩衝材必要枚数（0=不要）
End Class
```

#### 4.1.2 MaterialCondition（部材条件）

```vb
Public Class MaterialCondition
    Public Property CardNo As String           ' カードNo (FK)
    Public Property LapThickness As String     ' LAP厚
    Public Property Pre10mm As Integer         ' 投入前10mm必要枚数
    Public Property Post1mm As Integer         ' 投入後1mm必要枚数
    Public Property Post5mm As Integer         ' 投入後5mm必要枚数
    Public Property Post10mm As Integer        ' 投入後10mm必要枚数
End Class
```

#### 4.1.3 SessionState（セッション状態）

```vb
Public Class SessionState
    Public Property Timestamp As DateTime      ' タイムスタンプ
    Public Property Stage As Integer           ' 照合段階 (0-2)
    Public Property EmployeeNo As String       ' 従業員No
    Public Property EmployeeName As String     ' 従業員名
    Public Property CardNo As String           ' カードNo
    Public Property LapThickness As String     ' LAP厚
    Public Property Pre10mmBefore As Integer   ' 投入前10mm照合前数
    Public Property Pre10mmAfter As Integer    ' 投入前10mm照合後数
    Public Property Pre10mmShortage As Integer ' 投入前10mm過不足数
    Public Property Pre10mmJudgment As String  ' 投入前10mm判定結果
    Public Property CardConditionJson As String     ' カード条件JSON
    Public Property MaterialConditionJson As String ' 部材条件JSON
End Class
```

#### 4.1.4 ShelfStorage（棚入庫情報）

```vb
Public Class ShelfStorage
    Public Property StorageId As String        ' 入庫ID (GUID)
    Public Property CardNo As String           ' カードNo
    Public Property ProductName As String      ' 品名
    Public Property LotNo As String            ' ロットNo
    Public Property Quantity As Integer        ' 枚数
    Public Property Location As String         ' 工程
    Public Property ShelfNumber As String      ' 棚番号
    Public Property StorageDate As DateTime    ' 入庫日時
    Public Property IsRetrieved As Boolean     ' 出庫済みフラグ
End Class
```

### 4.2 ファイル設計

#### 4.2.1 設定ファイル（App.config）

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <!-- シリアルポート設定 -->
    <add key="SerialPortName" value="COM3" />
    <add key="BaudRate" value="9600" />
    
    <!-- ソケット通信設定 -->
    <add key="UseSocketForEmployee" value="false" />
    <add key="EmployeeServerIP" value="192.168.1.100" />
    <add key="EmployeeServerPort" value="5001" />
    
    <add key="UseSocketForCard" value="false" />
    <add key="CardServerIP" value="192.168.1.100" />
    <add key="CardServerPort" value="5002" />
    
    <!-- ログ設定 -->
    <add key="LogDirectory" value="logs" />
    <add key="ErrorLogDirectory" value="logs/error" />
  </appSettings>
</configuration>
```

#### 4.2.2 カード条件マスタ（card_conditions.csv）

```csv
CardNo,ProductName,LotNo,Quantity,Location,EdgeGuard,BubbleInterference
E00123,AAA,000001###,25,AAA,0,0
E00124,BBB,000002###,30,BBB,1,0
```

**カラム定義**:
| カラム名 | データ型 | 必須 | 説明 |
|---------|---------|------|------|
| CardNo | String(6) | ○ | カード番号 |
| ProductName | String | ○ | 品名 |
| LotNo | String | ○ | ロット番号 |
| Quantity | Integer | ○ | 枚数 |
| Location | String | ○ | 工程 |
| EdgeGuard | Integer | ○ | エッジガード必要枚数（0=不要） |
| BubbleInterference | Integer | ○ | 気泡緩衝材必要枚数（0=不要） |

#### 4.2.3 部材条件マスタ（material_conditions.csv）

```csv
CardNo,LapThickness,Pre10mm,Post1mm,Post5mm,Post10mm
E00123,250μm,1,1,2,1
E00123,150μm,2,1,3,1
```

**カラム定義**:
| カラム名 | データ型 | 必須 | 説明 |
|---------|---------|------|------|
| CardNo | String(6) | ○ | カード番号 |
| LapThickness | String | ○ | LAP厚（250μm等） |
| Pre10mm | Integer | ○ | 投入前10mm必要枚数 |
| Post1mm | Integer | ○ | 投入後1mm必要枚数 |
| Post5mm | Integer | ○ | 投入後5mm必要枚数 |
| Post10mm | Integer | ○ | 投入後10mm必要枚数 |

---

## 5. 処理フロー設計

### 5.1 起動時処理フロー

```
[システム起動]
    ↓
[InitializeServices]
├─ ConfigLoader初期化
├─ SerialPortManager初期化（天秤接続）
├─ EmployeeLoader初期化
├─ CardConditionLoader初期化
├─ MaterialConditionLoader初期化
├─ LogManager初期化
├─ ShelfStorageManager初期化
└─ SessionStateManager初期化
    ↓
[CheckAndRestoreSession]
    ↓
session_state.json存在？
    ├─ Yes → [復元確認ダイアログ表示]
    │         ├─ はい → RestoreSession実行
    │         └─ いいえ → セッションファイル削除
    └─ No  → [通常起動]
    ↓
[メイン画面表示]
```

### 5.2 第1段階照合処理フロー

```
[照合ボタン押下]
    ↓
[PerformFirstStageVerification]
    ↓
[天秤から投入前10mm計測]
├─ 照合前数取得（lblPre10mmRemaining）
├─ BalanceManager.GetWeight()
├─ ConvertWeightToCount(weight, 1.15g)
└─ 照合後数算出
    ↓
[過不足計算]
過不足 = 必要枚数 - 照合後数
    ↓
判定
├─ 過不足 = 0 → [OK]
│   ├─ メッセージ表示（緑色）
│   ├─ SaveSessionState(照合前数, 照合後数, 過不足)
│   ├─ _verificationStage = 1
│   └─ DisplayPostMaterialsForSecondStage()
└─ 過不足 ≠ 0 → [NG]
    ├─ メッセージ表示（赤色）
    └─ ログ記録（NG）
```

### 5.3 第2段階照合処理フロー

```
[照合ボタン押下（2回目）]
    ↓
[PerformSecondStageVerification]
    ↓
[各部材の計測]
For Each 部材 In [投入後1mm, 5mm, 10mm, エッジガード, 気泡緩衝材]
    ↓
    [天秤から重量取得]
    ├─ BalanceManager.GetWeight()
    ├─ ConvertWeightToCount(weight, 部材別重量)
    └─ 過不足計算
    ↓
    判定
    ├─ OK → 次の部材へ
    └─ NG → エラーメッセージ表示、処理中断
Next
    ↓
全部材OK？
├─ Yes → [照合完了]
│   ├─ メッセージ表示（緑色）
│   ├─ ログ記録（OK）
│   ├─ SessionStateManager.DeleteState()
│   └─ 棚入庫ボタン活性化
└─ No  → [照合失敗]
    └─ ログ記録（NG）
```

### 5.4 セッション復元処理フロー

```
[RestoreSession(savedState)]
    ↓
[イベント無効化]
RemoveHandler txtEmployeeNo.TextChanged
    ↓
[従業員情報復元]
├─ txtEmployeeNo.Text = savedState.EmployeeNo
├─ lblEmployeeNameValue.Text = savedState.EmployeeName
└─ txtEmployeeNo.Enabled = False
    ↓
[イベント再有効化]
AddHandler txtEmployeeNo.TextChanged
    ↓
[カード情報復元]
├─ txtCardNo.Text = savedState.CardNo
├─ txtCardNo.Enabled = False
└─ cmbLapThickness.SelectedItem = savedState.LapThickness
    ↓
[条件情報復元]
├─ JSON → CardCondition
├─ JSON → MaterialCondition
└─ 画面表示
    ↓
[投入前10mm照合結果復元]
├─ lblPre10mmRemaining.Text = savedState.Pre10mmBefore
├─ lblPre10mmSecured.Text = savedState.Pre10mmAfter
├─ lblPre10mmUsed.Text = savedState.Pre10mmShortage
├─ lblPre10mmJudgment.Text = savedState.Pre10mmJudgment
└─ 判定結果の色設定
    ↓
[投入後部材情報取得]
DisplayPostMaterialsForSecondStage()
    ↓
[メッセージ表示]
"照合OK。投入前10mmをプロトスに入れて移載してください..."
    ↓
[状態設定]
_verificationStage = 1
```

---

## 6. 例外処理設計

### 6.1 例外処理方針

1. **予測可能な例外**: Try-Catchで捕捉し、ユーザーにわかりやすいメッセージを表示
2. **システムエラー**: エラーログに詳細を記録し、アプリケーションは継続
3. **致命的エラー**: MessageBox表示後、アプリケーション終了

### 6.2 例外処理一覧

| 例外種類 | 発生箇所 | 処理内容 |
|---------|---------|---------|
| `FileNotFoundException` | CSV読込時 | エラーメッセージ表示、該当機能を無効化 |
| `IOException` | ファイル書込時 | エラーログ記録、処理継続 |
| `JsonException` | セッション復元時 | セッションファイル削除、通常起動 |
| `TimeoutException` | TCP通信時 | エラーメッセージ表示、CSVフォールバック |
| `UnauthorizedAccessException` | ファイルアクセス時 | 権限エラー表示、処理中断 |
| `InvalidOperationException` | シリアルポート時 | ポート接続エラー表示、再接続試行 |
| `FormatException` | データ変換時 | データ形式エラー表示、処理中断 |

### 6.3 例外処理例

```vb
Try
    Dim weight As Double = _balanceManager.GetWeight()
    Dim count As Integer = _balanceManager.ConvertWeightToCount(weight, 1.15)
Catch ex As TimeoutException
    ShowMessage("天秤通信タイムアウト", Color.Red)
    _logManager.WriteErrorLog("天秤通信エラー: " & ex.Message)
Catch ex As InvalidOperationException
    ShowMessage("天秤接続エラー", Color.Red)
    _logManager.WriteErrorLog("シリアルポートエラー: " & ex.Message)
Catch ex As Exception
    ShowMessage("計測エラー", Color.Red)
    _logManager.WriteErrorLog("予期しないエラー: " & ex.Message)
End Try
```

---

## 7. パフォーマンス設計

### 7.1 性能要件

| 項目 | 目標値 | 測定方法 |
|------|--------|---------|
| 天秤データ取得 | 5秒以内 | シリアル通信レスポンスタイム測定 |
| カード条件取得 | 3秒以内 | TCP通信またはCSV読込時間測定 |
| 画面表示更新 | 1秒以内 | UI更新処理時間測定 |
| セッション保存 | 1秒以内 | JSON書込時間測定 |

### 7.2 最適化戦略

1. **データキャッシング**: カード条件、部材条件を初回読込時にメモリキャッシュ
2. **非同期処理**: 従業員情報取得時に非同期実行（`Async/Await`）
3. **リソース管理**: `Using`ステートメントでファイル・通信リソースを確実に解放
4. **遅延読込**: 棚入庫・出庫データは画面表示時のみ読込

---

## 8. セキュリティ設計

### 8.1 セキュリティ要件

1. **アクセス制御**: 従業員認証による操作者の記録
2. **データ完全性**: ログファイルのタイムスタンプ記録
3. **エラーハンドリング**: 詳細なエラー情報はログファイルにのみ記録

### 8.2 セキュリティ対策

| 項目 | 対策内容 |
|------|---------|
| ログ記録 | すべての照合操作を従業員Noとともに記録 |
| ファイルアクセス | ローカルファイルシステムのみアクセス |
| 通信暗号化 | なし（ローカルネットワーク前提） |
| 入力検証 | 従業員No、カードNoの桁数チェック |

---

## 9. テスト設計

### 9.1 単体テスト項目

| テスト対象 | テスト項目 | 期待結果 |
|-----------|-----------|---------|
| BalanceManager | 重量→枚数変換（正常系） | 正しい枚数が返却される |
| BalanceManager | 重量→枚数変換（0g） | 0個が返却される |
| SessionStateManager | セッション保存 | JSONファイルが作成される |
| SessionStateManager | セッション読込 | SessionStateオブジェクトが返却される |
| SessionStateManager | 破損ファイル読込 | Nothingが返却され、ファイルが削除される |

### 9.2 結合テスト項目

| テスト対象 | テスト項目 | 期待結果 |
|-----------|-----------|---------|
| 第1段階照合 | OK判定 | メッセージ表示、セッション保存、第2段階へ遷移 |
| 第1段階照合 | NG判定（不足） | エラーメッセージ表示、ログ記録 |
| 第2段階照合 | 全部材OK | 照合完了メッセージ、ログ記録、セッション削除 |
| セッション復元 | 復元成功 | 状態復元、メッセージ表示、カードNo非活性 |

### 9.3 システムテスト項目

| テスト分類 | テスト項目 | 期待結果 |
|-----------|-----------|---------|
| 機能テスト | 通常照合フロー | エラーなく照合完了 |
| 機能テスト | セッション復元フロー | 異常終了後も状態復元可能 |
| 性能テスト | 天秤データ取得時間 | 5秒以内 |
| 負荷テスト | 連続照合100回 | エラーなく完了 |

---

## 改訂履歴

| 版数 | 改訂日 | 改訂者 | 改訂内容 |
|------|--------|--------|----------|
| 1.0 | 2025-11-10 | TakumaYamaguchi | 初版作成 |

