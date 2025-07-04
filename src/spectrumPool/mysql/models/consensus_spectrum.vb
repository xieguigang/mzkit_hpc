#Region "Microsoft.VisualBasic::e1b49b0f0dc600179bfa7e3384d135b4, Rscript\Library\mzkit_hpc\src\spectrumPool\mysql\models\consensus_spectrum.vb"

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

    '   Total Lines: 232
    '    Code Lines: 108 (46.55%)
    ' Comment Lines: 102 (43.97%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 22 (9.48%)
    '     File Size: 14.82 KB


    ' Class consensus_spectrum
    ' 
    '     Properties: add_time, adducts, cluster_id, consensus_entropy, formula
    '                 id, intensity, mz, parameter_id, peak_ranking
    '                 precursor, precursor_entropy, rt, source_size, splash_id
    '                 umap
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
''' the spectrum data that extract from a cluster data
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("consensus_spectrum", Database:="sample_pool", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `consensus_spectrum` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `cluster_id` INT UNSIGNED NOT NULL,
  `parameter_id` INT UNSIGNED NOT NULL COMMENT 'parameter id reference of cutoff range and umap parameters',
  `source_size` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'the source cluster size for construct this consensus spectrum. use this parameter for make verify of the cluster data updates(cluster size will change if new spectrum has been added)',
  `precursor` DOUBLE UNSIGNED NOT NULL,
  `precursor_entropy` DOUBLE NOT NULL DEFAULT 0 COMMENT 'entropy of the precursor in this cluster',
  `consensus_entropy` DOUBLE NOT NULL DEFAULT 0 COMMENT 'entropy of the consensus spectrum',
  `splash_id` VARCHAR(45) NOT NULL COMMENT 'splash id of the consensus spectrum',
  `formula` VARCHAR(255) NULL COMMENT 'the highest possible annotated formula of this cluster',
  `adducts` VARCHAR(45) NULL,
  `rt` DOUBLE NULL,
  `mz` LONGTEXT NOT NULL COMMENT 'consensus peak and intensity value, base64strng encoded of double number in network byteorder',
  `intensity` LONGTEXT NOT NULL COMMENT 'consensus peak and intensity value, base64strng encoded of double number in network byteorder',
  `peak_ranking` LONGTEXT NOT NULL COMMENT 'consensus peak annotation and ranking value, base64 string encoded double numbers in network byteorder',
  `umap` VARCHAR(2048) NOT NULL COMMENT 'base64string for umap result of multiple dimension number encoded in network byte order',
  `add_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `cluster_info_idx` (`cluster_id` ASC) VISIBLE,
  INDEX `parameter_info_idx` (`parameter_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3
COMMENT = 'the spectrum data that extract from a cluster data';
")>
Public Class consensus_spectrum: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("cluster_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="cluster_id")> Public Property cluster_id As UInteger
''' <summary>
''' parameter id reference of cutoff range and umap parameters
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("parameter_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="parameter_id")> Public Property parameter_id As UInteger
''' <summary>
''' the source cluster size for construct this consensus spectrum. use this parameter for make verify of the cluster data updates(cluster size will change if new spectrum has been added)
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("source_size"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="source_size")> Public Property source_size As UInteger
    <DatabaseField("precursor"), NotNull, DataType(MySqlDbType.Double), Column(Name:="precursor")> Public Property precursor As Double
''' <summary>
''' entropy of the precursor in this cluster
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("precursor_entropy"), NotNull, DataType(MySqlDbType.Double), Column(Name:="precursor_entropy")> Public Property precursor_entropy As Double
''' <summary>
''' entropy of the consensus spectrum
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("consensus_entropy"), NotNull, DataType(MySqlDbType.Double), Column(Name:="consensus_entropy")> Public Property consensus_entropy As Double
''' <summary>
''' splash id of the consensus spectrum
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("splash_id"), NotNull, DataType(MySqlDbType.VarChar, "45"), Column(Name:="splash_id")> Public Property splash_id As String
''' <summary>
''' the highest possible annotated formula of this cluster
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("formula"), DataType(MySqlDbType.VarChar, "255"), Column(Name:="formula")> Public Property formula As String
    <DatabaseField("adducts"), DataType(MySqlDbType.VarChar, "45"), Column(Name:="adducts")> Public Property adducts As String
    <DatabaseField("rt"), DataType(MySqlDbType.Double), Column(Name:="rt")> Public Property rt As Double
''' <summary>
''' consensus peak and intensity value, base64strng encoded of double number in network byteorder
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("mz"), NotNull, DataType(MySqlDbType.Text), Column(Name:="mz")> Public Property mz As String
''' <summary>
''' consensus peak and intensity value, base64strng encoded of double number in network byteorder
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("intensity"), NotNull, DataType(MySqlDbType.Text), Column(Name:="intensity")> Public Property intensity As String
''' <summary>
''' consensus peak annotation and ranking value, base64 string encoded double numbers in network byteorder
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("peak_ranking"), NotNull, DataType(MySqlDbType.Text), Column(Name:="peak_ranking")> Public Property peak_ranking As String
''' <summary>
''' base64string for umap result of multiple dimension number encoded in network byte order
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("umap"), NotNull, DataType(MySqlDbType.VarChar, "2048"), Column(Name:="umap")> Public Property umap As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `consensus_spectrum` (`cluster_id`, `parameter_id`, `source_size`, `precursor`, `precursor_entropy`, `consensus_entropy`, `splash_id`, `formula`, `adducts`, `rt`, `mz`, `intensity`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `source_size`, `precursor`, `precursor_entropy`, `consensus_entropy`, `splash_id`, `formula`, `adducts`, `rt`, `mz`, `intensity`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `consensus_spectrum` (`cluster_id`, `parameter_id`, `source_size`, `precursor`, `precursor_entropy`, `consensus_entropy`, `splash_id`, `formula`, `adducts`, `rt`, `mz`, `intensity`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `source_size`, `precursor`, `precursor_entropy`, `consensus_entropy`, `splash_id`, `formula`, `adducts`, `rt`, `mz`, `intensity`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `consensus_spectrum` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `consensus_spectrum` SET `id`='{0}', `cluster_id`='{1}', `parameter_id`='{2}', `source_size`='{3}', `precursor`='{4}', `precursor_entropy`='{5}', `consensus_entropy`='{6}', `splash_id`='{7}', `formula`='{8}', `adducts`='{9}', `rt`='{10}', `mz`='{11}', `intensity`='{12}', `peak_ranking`='{13}', `umap`='{14}', `add_time`='{15}' WHERE `id` = '{16}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `consensus_spectrum` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `source_size`, `precursor`, `precursor_entropy`, `consensus_entropy`, `splash_id`, `formula`, `adducts`, `rt`, `mz`, `intensity`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, cluster_id, parameter_id, source_size, precursor, precursor_entropy, consensus_entropy, splash_id, formula, adducts, rt, mz, intensity, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `source_size`, `precursor`, `precursor_entropy`, `consensus_entropy`, `splash_id`, `formula`, `adducts`, `rt`, `mz`, `intensity`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, cluster_id, parameter_id, source_size, precursor, precursor_entropy, consensus_entropy, splash_id, formula, adducts, rt, mz, intensity, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, cluster_id, parameter_id, source_size, precursor, precursor_entropy, consensus_entropy, splash_id, formula, adducts, rt, mz, intensity, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{cluster_id}', '{parameter_id}', '{source_size}', '{precursor}', '{precursor_entropy}', '{consensus_entropy}', '{splash_id}', '{formula}', '{adducts}', '{rt}', '{mz}', '{intensity}', '{peak_ranking}', '{umap}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{cluster_id}', '{parameter_id}', '{source_size}', '{precursor}', '{precursor_entropy}', '{consensus_entropy}', '{splash_id}', '{formula}', '{adducts}', '{rt}', '{mz}', '{intensity}', '{peak_ranking}', '{umap}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `source_size`, `precursor`, `precursor_entropy`, `consensus_entropy`, `splash_id`, `formula`, `adducts`, `rt`, `mz`, `intensity`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, cluster_id, parameter_id, source_size, precursor, precursor_entropy, consensus_entropy, splash_id, formula, adducts, rt, mz, intensity, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `source_size`, `precursor`, `precursor_entropy`, `consensus_entropy`, `splash_id`, `formula`, `adducts`, `rt`, `mz`, `intensity`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, cluster_id, parameter_id, source_size, precursor, precursor_entropy, consensus_entropy, splash_id, formula, adducts, rt, mz, intensity, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, cluster_id, parameter_id, source_size, precursor, precursor_entropy, consensus_entropy, splash_id, formula, adducts, rt, mz, intensity, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `consensus_spectrum` SET `id`='{0}', `cluster_id`='{1}', `parameter_id`='{2}', `source_size`='{3}', `precursor`='{4}', `precursor_entropy`='{5}', `consensus_entropy`='{6}', `splash_id`='{7}', `formula`='{8}', `adducts`='{9}', `rt`='{10}', `mz`='{11}', `intensity`='{12}', `peak_ranking`='{13}', `umap`='{14}', `add_time`='{15}' WHERE `id` = '{16}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, cluster_id, parameter_id, source_size, precursor, precursor_entropy, consensus_entropy, splash_id, formula, adducts, rt, mz, intensity, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As consensus_spectrum
                         Return DirectCast(MyClass.MemberwiseClone, consensus_spectrum)
                     End Function
End Class


End Namespace
