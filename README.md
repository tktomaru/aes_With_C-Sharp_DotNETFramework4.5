# aes_With_C-Sharp_DotNETFramework4.5
.NET Framework 4.5を使ってaesによる暗復号を行う

# <概要>

aes.exeを実行するとaesで暗号化されたデータを復号することができる。
また、平文を暗号化することができる。

# <実行に必要なファイル>

## プログラム本体「aes.exe」**※必須**

暗号化／復号を行うプログラム本体

<br>

## 第1引数 **※必須**

* 暗号化／復号のどちらを行うか指定する
* 0 encode
* 1 decode

## 第2引数 データファイル「data.txt」**※必須**

* 暗号化／復号したいデータを記載する
* ファイル名は任意
  
## 第3引数 鍵ファイル「key.dat」**※必須**

* 暗号化／復号する際の鍵ファイルを指定する
* ファイル名は任意
 
## 第4引数 出力ファイル「out-enc.dat」**※必須**

* 暗号化／復号した後の出力ファイルを指定する
* ファイル名は任意
 
## 第5引数 初期ベクトルファイル「iv.dat」**※任意**

* 暗号化／復号する際の初期ベクトルを指定する
* ファイル名は任意
 
## 第6引数 argCypherMode **※任意**

* CBC = 1
* ECB = 2
* OFB = 3
* CFB = 4
* CTS = 5
 
## 第7引数 argPaddingMode **※任意**

* None = 1 埋め込みなし
* PKCS7 = 2
* Zeros = 3
* ANSIX923 = 4
* ISO10126 = 5
 
## 第8引数 argKeySizes **※任意**

* 128 がデフォルト
 
## 第9引数 argBlockSize **※任意**

* 128 がデフォルト

<br>

# <使い方>

## 暗号化する

rem ■暗号化する
rem 0 enc dec flg 1 = encode
rem 1 data
rem 2 key
rem 3 out
rem 4 iv
rem 5 argCypherMode CBC = 1 ECB = 2 OFB = 3 CFB = 4 CTS = 5
rem 6 argPaddingMode  None = 1 埋め込みなし PKCS7 = 2 Zeros = 3 ANSIX923 = 4 ISO10126 = 5
rem 7 argKeySizes  128 がデフォルト
rem 8 argBlockSize 128 がデフォルト
aes 1 data.txt key.dat out-enc.dat iv.dat 1 3 128 128

## 復号する

rem ■上記で暗号化したデータを復号する
rem decode
rem 0 enc dec flg 0 = decode
rem 1 data
rem 2 key
rem 3 out
rem 4 iv
rem 5 argCypherMode CBC = 1 ECB = 2 OFB = 3 CFB = 4 CTS = 5
rem 6 argPaddingMode  None = 1 埋め込みなし PKCS7 = 2 Zeros = 3 ANSIX923 = 4 ISO10126 = 5
rem 7 argKeySizes  128 がデフォルト
rem 8 argBlockSize 128 がデフォルト
aes 0 out-enc.dat key.dat out-dec.dat iv.dat 1 3

<br>
