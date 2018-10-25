Imports System.IO
Imports System.Security.Cryptography
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/rsa.key")>
    <Usage("/rsa.key [/out <default=./>]")>
    Public Function CreateRSAKeys(args As CommandLine) As Integer
        Dim rsa As RSACryptoServiceProvider = New RSACryptoServiceProvider
        Dim dir As String = args("/out") Or "./"

        Using writer As StreamWriter = $"{dir}/PrivateKey.xml".OpenWriter
            ' 这个文件是需要进行保密的密匙
            writer.WriteLine(rsa.ToXmlString(True))
        End Using
        Using writer As StreamWriter = $"{dir}/PublicKey.xml".OpenWriter
            ' 这个是分发给其他人的公匙
            writer.WriteLine(rsa.ToXmlString(False))
        End Using

        Return 0
    End Function

    <ExportAPI("/rsa.encrypt")>
    <Usage("/rsa.encrypt /message <text> /private <privatekey.xml> [/out <out.txt>]")>
    Public Function PrivateEncrypt(args As CommandLine) As Integer

    End Function
End Module
