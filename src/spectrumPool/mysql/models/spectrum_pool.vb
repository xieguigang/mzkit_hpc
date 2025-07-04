#Region "Microsoft.VisualBasic::7a3363489d7ad04e41f6e77f248b874d, Rscript\Library\mzkit_hpc\src\spectrumPool\mysql\models\spectrum_pool.vb"

    ' Author:
    ' 
    '       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
    ' 
    ' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 184
    '    Code Lines: 96 (52.17%)
    ' Comment Lines: 66 (35.87%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 22 (11.96%)
    '     File Size: 9.23 KB


    ' Class spectrum_pool
    ' 
    '     Properties: add_time, entropy, hashcode, id, into
    '                 model_id, mz, npeaks, splash_id
    ' 
    '     Function: Clone, GetDeleteSQL, GetDumpInsertValue, (+2 Overloads) GetInsertSQL, (+2 Overloads) GetReplaceSQL
    '               GetUpdateSQL
    ' 
    ' 
    ' /********************************************************************************/

#End Region

REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @5/28/2025 02:55:31 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace clusterModels

''' <summary>
''' ```SQL
''' the spectrum data pool
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("spectrum_pool", Database:="sample_pool", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `spectrum_pool` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `npeaks` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'number of ion peaks in current spectrum data',
  `entropy` DOUBLE NOT NULL DEFAULT '0',
  `splash_id` VARCHAR(45) NOT NULL,
  `hashcode` CHAR(32) NOT NULL COMMENT 'md5 hash code of the spectrum data',
  `model_id` INT UNSIGNED NOT NULL,
  `mz` LONGTEXT NOT NULL COMMENT 'mz double array data in network byte order and base64 encoding, andalso the byte data is compress in gzip',
  `into` LONGTEXT NOT NULL COMMENT 'intensity double array data in network byte order and base64 encoding, andalso the byte data is compress in gzip',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `model_index` (`model_id` ASC) VISIBLE,
  INDEX `peak_hash_index` (`hashcode` ASC) VISIBLE,
  INDEX `find_spectrum1` (`id` ASC, `model_id` ASC) VISIBLE,
  INDEX `find_spectrum2` (`hashcode` ASC, `model_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3
COMMENT = 'the spectrum data pool';
")>
Public Class spectrum_pool: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' number of ion peaks in current spectrum data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("npeaks"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="npeaks")> Public Property npeaks As UInteger
    <DatabaseField("entropy"), NotNull, DataType(MySqlDbType.Double), Column(Name:="entropy")> Public Property entropy As Double
    <DatabaseField("splash_id"), NotNull, DataType(MySqlDbType.VarChar, "45"), Column(Name:="splash_id")> Public Property splash_id As String
''' <summary>
''' md5 hash code of the spectrum data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
    <DatabaseField("model_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="model_id")> Public Property model_id As UInteger
''' <summary>
''' mz double array data in network byte order and base64 encoding, andalso the byte data is compress in gzip
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("mz"), NotNull, DataType(MySqlDbType.Text), Column(Name:="mz")> Public Property mz As String
''' <summary>
''' intensity double array data in network byte order and base64 encoding, andalso the byte data is compress in gzip
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("into"), NotNull, DataType(MySqlDbType.Text), Column(Name:="into")> Public Property into As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `spectrum_pool` (`npeaks`, `entropy`, `splash_id`, `hashcode`, `model_id`, `mz`, `into`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `spectrum_pool` (`id`, `npeaks`, `entropy`, `splash_id`, `hashcode`, `model_id`, `mz`, `into`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `spectrum_pool` (`npeaks`, `entropy`, `splash_id`, `hashcode`, `model_id`, `mz`, `into`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `spectrum_pool` (`id`, `npeaks`, `entropy`, `splash_id`, `hashcode`, `model_id`, `mz`, `into`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `spectrum_pool` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `spectrum_pool` SET `id`='{0}', `npeaks`='{1}', `entropy`='{2}', `splash_id`='{3}', `hashcode`='{4}', `model_id`='{5}', `mz`='{6}', `into`='{7}', `add_time`='{8}' WHERE `id` = '{9}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `spectrum_pool` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `spectrum_pool` (`id`, `npeaks`, `entropy`, `splash_id`, `hashcode`, `model_id`, `mz`, `into`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, npeaks, entropy, splash_id, hashcode, model_id, mz, into, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `spectrum_pool` (`id`, `npeaks`, `entropy`, `splash_id`, `hashcode`, `model_id`, `mz`, `into`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, npeaks, entropy, splash_id, hashcode, model_id, mz, into, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, npeaks, entropy, splash_id, hashcode, model_id, mz, into, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{npeaks}', '{entropy}', '{splash_id}', '{hashcode}', '{model_id}', '{mz}', '{into}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{npeaks}', '{entropy}', '{splash_id}', '{hashcode}', '{model_id}', '{mz}', '{into}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `spectrum_pool` (`id`, `npeaks`, `entropy`, `splash_id`, `hashcode`, `model_id`, `mz`, `into`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, npeaks, entropy, splash_id, hashcode, model_id, mz, into, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `spectrum_pool` (`id`, `npeaks`, `entropy`, `splash_id`, `hashcode`, `model_id`, `mz`, `into`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, npeaks, entropy, splash_id, hashcode, model_id, mz, into, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, npeaks, entropy, splash_id, hashcode, model_id, mz, into, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `spectrum_pool` SET `id`='{0}', `npeaks`='{1}', `entropy`='{2}', `splash_id`='{3}', `hashcode`='{4}', `model_id`='{5}', `mz`='{6}', `into`='{7}', `add_time`='{8}' WHERE `id` = '{9}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, npeaks, entropy, splash_id, hashcode, model_id, mz, into, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As spectrum_pool
                         Return DirectCast(MyClass.MemberwiseClone, spectrum_pool)
                     End Function
End Class


End Namespace
