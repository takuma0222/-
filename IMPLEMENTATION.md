# 従業員NOチェック機能 実装ドキュメント

## 概要
従業員NOを6桁入力した際に、ローカルCSVファイルから氏名を検索して画面に表示する機能を実装しました。

## 実装内容

### 1. 追加・変更したファイル

#### 新規追加ファイル
1. **`BalanceInspection/Services/EmployeeLoader.vb`**
   - 従業員データCSVの非同期読み込みクラス
   - 主なメソッド:
     - `LoadAsync()`: CSVファイルを非同期で読み込み
     - `GetEmployeeNameAsync(employeeNo As String)`: 従業員NOから氏名を非同期で取得
     - `ReloadAsync()`: データを再読み込み

2. **`BalanceInspection/Models/Employee.vb`**
   - 従業員データモデル（将来の拡張用）
   - プロパティ: EmployeeNo, Name

3. **`BalanceInspection/data/employees.csv`**
   - 従業員NOと氏名のマッピングデータ
   - フォーマット: UTF-8, カンマ区切り, LF改行
   - サンプルデータ含む（123456,山田太郎 等）

4. **`BalanceInspection/data/README.md`**
   - 従業員データCSVの仕様とExcel編集方法を記載

#### 変更ファイル
1. **`BalanceInspection/MainForm.vb`**
   - UIコンポーネント追加:
     - `lblEmployeeName`: 従業員名表示用ラベル（青文字）
     - `lblLoading`: ローディング表示用ラベル（グレー、イタリック）
   
   - サービス追加:
     - `_employeeLoader`: EmployeeLoaderのインスタンス
   
   - メソッド追加/変更:
     - `EmployeeNo_KeyPress`: 従業員NO入力時に数字のみ許可
     - `TxtEmployeeNo_TextChanged`: 非同期従業員検索処理（Async Sub）
     - `ShowLoading`: ローディング表示の切り替え
     - `ResetForm`: 従業員名ラベルのクリア処理を追加
   
   - 変更箇所の詳細:
     - Line 9-17: UIコンポーネントとサービスのフィールド追加
     - Line 56-62: 従業員名ラベルの初期化
     - Line 111-118: ローディングラベルの初期化とDataGridViewの位置調整
     - Line 157-160: EmployeeLoader初期化処理
     - Line 182-191: 従業員NO用KeyPressハンドラー追加
     - Line 370-384: ResetFormメソッドに従業員名クリア処理追加
     - Line 397-413: ShowLoadingメソッド追加
     - Line 421-514: TxtEmployeeNo_TextChangedメソッドを非同期対応に変更

2. **`BalanceInspection/Models/AppConfig.vb`**
   - `EmployeeCsvPath` プロパティを追加（デフォルト: "data\employees.csv"）

3. **`BalanceInspection/BalanceInspection.vbproj`**
   - Employee.vb と EmployeeLoader.vb をコンパイル対象に追加

### 2. UI変更詳細

#### 変更したコントロール
- **従業員Noテキストボックス (txtEmployeeNo)**
  - 位置: (130, 20)
  - サイズ: 150x20
  - MaxLength: 6
  - KeyPressイベント: EmployeeNo_KeyPress（数字のみ）
  - TextChangedイベント: TxtEmployeeNo_TextChanged（非同期）

#### 追加したコントロール
- **従業員名ラベル (lblEmployeeName)**
  - 位置: (130, 45) - 従業員Noテキストボックスの下
  - サイズ: 150x20
  - フォント: 9pt, Regular
  - 色: 青文字（該当時）/ 赤文字（該当なし時）

- **ローディングラベル (lblLoading)**
  - 位置: (290, 20)
  - サイズ: 100x20
  - フォント: 8pt, Italic
  - 色: グレー
  - 初期状態: 非表示

#### レイアウト調整
- メッセージラベル: Y座標を60→70に変更
- DataGridView: Y座標を110→120に変更（従業員名ラベルのスペース確保）

### 3. 処理フロー

#### 従業員NO入力時（6桁入力完了時）
```
1. ユーザが6桁目を入力
   ↓
2. TxtEmployeeNo_TextChanged イベント発火
   ↓
3. 数字6桁かチェック
   - 数字以外 → エラーメッセージ表示、処理中断
   ↓
4. ローディング表示 "読み込み中..." を表示
   ↓
5. _employeeLoader.GetEmployeeNameAsync(employeeNo) 非同期実行
   - 初回または未ロード時: CSVファイルを非同期読み込み
   - Dictionary から従業員NOで検索
   ↓
6. ローディング非表示
   ↓
7. 結果判定:
   【該当あり】
   - 従業員名を青文字で表示
   - カードNoフィールド活性化
   - フォーカスをカードNoに移動
   - メッセージ: "カードNoを入力してください"
   - ログ: 検索成功
   
   【該当なし】
   - "該当するユーザがありません。" を赤文字で表示（従業員名ラベル）
   - メッセージエリアにも同じメッセージを赤文字で表示
   - 従業員Noフィールドをクリア
   - フォーカスを従業員Noに戻す
   - カードNo非活性
   - ログ: 検索失敗（従業員NO含む）
   
   【エラー】
   - メッセージ: "データ読み取りエラーが発生しました"
   - 従業員Noフィールドをクリア
   - フォーカスを従業員Noに戻す
   - カードNo非活性
   - ログ: エラー詳細（スタックトレース含む）
```

### 4. ログ出力

既存の `LogManager.WriteErrorLog` を使用してログ出力:

| イベント | ログ内容 | 出力先 |
|---------|---------|-------|
| 検索成功 | `"従業員検索成功: {employeeNo} - {name}"` | logs/error/[日付].log |
| 検索失敗 | `"従業員検索失敗: 従業員NO {employeeNo} が見つかりません"` | logs/error/[日付].log |
| CSVエラー | `"従業員検索エラー: {ex.Message}\n{ex.StackTrace}"` | logs/error/[日付].log |

注: 現在のLogManagerにはInfoログがないため、成功時もWriteErrorLogを使用しています。

### 5. エラーハンドリング

#### ファイル未検出エラー
- **発生条件**: `data\employees.csv` が存在しない
- **ユーザ通知**: "データ読み取りエラーが発生しました"
- **ログ**: `CSV file not found: [パス]`
- **動作**: 入力欄クリア、フォーカス復帰

#### 読み取り失敗・パースエラー
- **発生条件**: ファイル読み込みエラー、不正なフォーマット
- **ユーザ通知**: "データ読み取りエラーが発生しました"
- **ログ**: 例外メッセージとスタックトレース
- **動作**: 入力欄クリア、フォーカス復帰

### 6. 非同期処理

#### 実装方法
- `Async Sub` を使用した非同期イベントハンドラー
- `Task.Run` でファイルI/O処理をバックグラウンドスレッドで実行
- UI更新はUIスレッドで自動実行（Async/Await パターン）

#### UIスレッド安全性
- `Await` によりUI更新は自動的にUIスレッドで実行される
- ローディング表示とUI更新のタイミングが適切に制御される

### 7. バリデーション

#### 入力制約
- **文字種制限**: 数字のみ（KeyPressイベントで制御）
- **桁数制限**: 6桁固定（MaxLength=6）
- **自動検索条件**: 6桁の数字が入力された時点

#### バリデーションロジック
```vb
' 6桁未満 → 何もしない
If employeeNo.Length < 6 Then
    lblEmployeeName.Text = ""
    Return
End If

' 6桁だが数字以外を含む → エラー
If Not IsNumeric(employeeNo) Then
    ShowMessage("従業員Noは数字のみで入力してください", Color.Red)
    Return
End If

' 6桁の数字 → 検索実行
```

## 動作確認手順

### 前提条件
1. Visual Studio または MSBuild がインストールされていること
2. .NET Framework 4.8 がインストールされていること

### ビルド手順
```bash
# ソリューションのビルド
msbuild BalanceInspection.sln /t:Build /p:Configuration=Debug
```

### CSV配置
```bash
# 実行ファイルと同じディレクトリに data フォルダを配置
cd BalanceInspection\bin\Debug
mkdir data
copy ..\..\..\data\employees.csv data\
```

### 実行とテスト
1. `BalanceInspection.exe` を実行
2. 従業員Noフィールドに「123456」と入力
3. 氏名「山田太郎」が表示されることを確認
4. 存在しない従業員NO「999999」を入力
5. 赤文字で「該当するユーザがありません。」が表示され、入力欄がクリアされることを確認
6. ログファイル `logs\error\[日付].log` で検索ログを確認

### サンプルCSVデータ
```csv
EmployeeNo,Name
123456,山田太郎
234567,佐藤花子
345678,鈴木一郎
456789,田中美咲
567890,高橋健太
```

## 既知の制約・注意点

### 1. CSV重複行の処理
- **現状**: 同じ従業員NOが複数行ある場合、最後に読み込まれた行のデータが有効
- **推奨**: CSVファイル作成時に従業員NOの重複をチェック

### 2. 大きなファイルのパフォーマンス
- **現状**: 全データをメモリに読み込む方式
- **影響**: 数万件を超えるとメモリ使用量が増加
- **対策案**: 必要に応じてデータベースへの移行を検討

### 3. ファイルロック
- **注意**: CSV編集中にアプリケーションを起動すると読み込みエラー
- **対策**: CSV編集後は必ずファイルを閉じてから実行

### 4. エンコーディング
- **指定**: UTF-8
- **注意**: Shift-JIS等で保存すると日本語が文字化け

### 5. キャッシュ
- **挙動**: 一度読み込んだCSVデータはメモリにキャッシュされる
- **更新**: アプリケーション再起動が必要（ReloadAsync メソッドで将来対応可能）

## 将来の拡張性

### 対応可能な拡張
1. **列の追加**: 部署名、役職等の情報追加
2. **エンコーディング対応**: Shift-JIS, EUC-JP等への対応
3. **ファイル配置変更**: 設定ファイルでパス変更可能（AppConfig.EmployeeCsvPath）
4. **データリロード機能**: ボタン追加で `_employeeLoader.ReloadAsync()` 呼び出し
5. **データベース移行**: EmployeeLoaderのインターフェース化でDB対応

### 推奨される改善
1. **Infoログの追加**: LogManagerにWriteInfoLogメソッドを追加
2. **単体テストの追加**: EmployeeLoaderクラスのテストケース実装（NUnit等）
3. **CSV検証ツール**: 重複チェック、フォーマット検証ツールの作成
4. **進行状況表示**: 大量データ読み込み時のプログレスバー表示

## セキュリティ考慮事項
- ファイルパスの検証（パストラバーサル対策）は既存のPath.Combineで実装済み
- CSVインジェクション対策は特に不要（データ表示のみ、コマンド実行なし）

## まとめ
既存のコードスタイルと命名規則に従い、最小限の変更で従業員NOチェック機能を実装しました。
非同期処理、エラーハンドリング、ログ出力、UIスレッド安全性を考慮した実装となっています。
