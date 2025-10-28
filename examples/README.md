# サンプル設定ファイル

このディレクトリには、アプリケーション実行に必要な設定ファイルのサンプルが含まれています。

## ファイル一覧

### appsettings.json
アプリケーション設定ファイルのサンプルです。
初回起動時に自動生成されますが、手動で作成する場合の参考にしてください。

**配置場所**: 実行ファイル（BalanceInspection.exe）と同じディレクトリ

**設定項目**:
- `LogDirectory`: ログ出力先ディレクトリ（相対パスまたは絶対パス）
- `CardConditionCsvPath`: カード条件CSVファイルのパス（相対パスまたは絶対パス）
- `ReadTimeoutMs`: 天秤からの応答待ちタイムアウト（ミリ秒）
- `MaxRetries`: タイムアウト時のリトライ回数
- `Balances`: 天秤設定の配列
  - `LogicalName`: 論理名（Pre_10mm, Post_1mm, Post_5mm など）
  - `PortName`: COMポート名（COM1, COM2, COM3 など）
  - `BaudRate`: ボーレート（1200, 2400, 4800, 9600）
  - `DataBits`: データビット（7, 8）
  - `Parity`: パリティ（None, Even, Odd）
  - `StopBits`: ストップビット（One, Two）
  - `DeviceId`: 機器ID（オプション）

**カスタマイズ例**:

タイムアウトを延長する場合:
```json
"ReadTimeoutMs": 10000
```

ボーレートを変更する場合:
```json
"BaudRate": 4800
```

パリティを変更する場合（7E1設定）:
```json
"DataBits": 7,
"Parity": "Even",
"StopBits": "One"
```

### card_conditions.csv
カード番号と使用部材条件の対応表のサンプルです。
初回起動時に自動生成されますが、実際の運用データに置き換えてください。

**配置場所**: appsettings.jsonの`CardConditionCsvPath`で指定した場所
（デフォルト: 実行ファイルと同じディレクトリ）

**フォーマット**:
- エンコーディング: UTF-8 BOM付き
- 1行目: ヘッダー行（固定）
- 2行目以降: データ行

**列の説明**:
1. CardNo: カード番号（6桁英数字、一意）
2. 投入前10mmクッション材: 個数（整数）
3. 投入後1mmクッション材: 個数（整数）※照合対象
4. 投入後5mmクッション材: 個数（整数）※照合対象
5. 投入後10mmクッション材: 個数（整数）
6. エッジガード: 個数（整数）
7. 気泡緩衝材: 個数（整数、2桁表示）
8. 品名: 製品名（文字列）
9. 枚数: ロット枚数（整数）
10. 所在: 保管場所（文字列）

**注意事項**:
- カード番号は重複しないこと
- 数値は0以上の整数
- 空欄は0として扱われます
- ファイルはExcelで編集可能ですが、保存時はUTF-8 BOMを選択してください

**Excelでの編集方法**:
1. Excelで card_conditions.csv を開く
2. データを編集
3. 「名前を付けて保存」→「CSV UTF-8（コンマ区切り）(*.csv)」を選択
4. 保存

## 使用方法

1. このディレクトリからサンプルファイルをコピー:
   ```
   copy examples\appsettings.json BalanceInspection\bin\Debug\
   copy examples\card_conditions.csv BalanceInspection\bin\Debug\
   ```

2. 実際の環境に合わせて設定を編集

3. アプリケーションを起動

## トラブルシューティング

### UTF-8 BOMで保存されていない場合
日本語が文字化けする可能性があります。
以下のツールでUTF-8 BOMに変換してください：
- Visual Studio Code: 右下の「UTF-8」をクリック → 「エンコード付きで保存」→ 「UTF-8 with BOM」
- メモ帳: 「名前を付けて保存」→ 文字コード: 「UTF-8」

### COMポート番号の確認方法
1. Windowsの「デバイスマネージャー」を開く
2. 「ポート (COMとLPT)」を展開
3. 接続されている電子天秤のCOMポート番号を確認
4. appsettings.json の PortName を更新
