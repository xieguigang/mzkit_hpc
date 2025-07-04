#Region "Microsoft.VisualBasic::374bba609f4dbda7d091fefa1003fe76, Rscript\Library\mzkit_hpc\src\spectrumPool\mysql\models\cluster_data.vb"

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

    '   Total Lines: 212
    '    Code Lines: 106 (50.00%)
    ' Comment Lines: 84 (39.62%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 22 (10.38%)
    '     File Size: 12.72 KB


    ' Class cluster_data
    ' 
    '     Properties: add_time, cluster_id, consensus, entropy, forward
    '                 id, jaccard, metadata_id, model_id, n_hits
    '                 p_value, reverse, score, spectral_id
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
''' the cluster data link of the spectrum members in current cluster tree node
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("cluster_data", Database:="sample_pool", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `cluster_data` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `cluster_id` INT UNSIGNED NOT NULL COMMENT 'the cluster tree node id, this id could be duplicated, this data field indicates that which cluster is belongs to of current spectram data',
  `model_id` INT UNSIGNED NOT NULL,
  `metadata_id` INT UNSIGNED NOT NULL COMMENT 'the metabolite annotation id of the current spectrum data',
  `spectral_id` INT UNSIGNED NOT NULL COMMENT 'the spectrum data reference id',
  `score` FLOAT NOT NULL DEFAULT '0' COMMENT 'the score value of current spectrum that align with the root spectrum of current cluster',
  `n_hits` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'number of the ms2 ion fragment hits in the spectrum aignment operation',
  `forward` FLOAT NOT NULL DEFAULT '0',
  `reverse` FLOAT NOT NULL DEFAULT '0',
  `jaccard` FLOAT NOT NULL DEFAULT '0',
  `entropy` FLOAT NOT NULL DEFAULT '0',
  `p_value` DOUBLE NOT NULL DEFAULT '1' COMMENT 't-test p-value of [forward,reverse,jaccard,entropy]',
  `consensus` LONGTEXT NULL DEFAULT NULL COMMENT 'consensus ions m/z value, network byte order data stream in base64 string',
  `add_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `model_index` (`model_id` ASC) VISIBLE,
  INDEX `meta_id` (`metadata_id` ASC) VISIBLE,
  INDEX `query_score_item` (`cluster_id` ASC, `spectral_id` ASC) VISIBLE,
  INDEX `find_spectra` (`spectral_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3
COMMENT = 'the cluster data link of the spectrum members in current cluster tree node';
")>
Public Class cluster_data: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the cluster tree node id, this id could be duplicated, this data field indicates that which cluster is belongs to of current spectram data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("cluster_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="cluster_id")> Public Property cluster_id As UInteger
    <DatabaseField("model_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="model_id")> Public Property model_id As UInteger
''' <summary>
''' the metabolite annotation id of the current spectrum data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("metadata_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="metadata_id")> Public Property metadata_id As UInteger
''' <summary>
''' the spectrum data reference id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("spectral_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="spectral_id")> Public Property spectral_id As UInteger
''' <summary>
''' the score value of current spectrum that align with the root spectrum of current cluster
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("score"), NotNull, DataType(MySqlDbType.Double), Column(Name:="score")> Public Property score As Double
''' <summary>
''' number of the ms2 ion fragment hits in the spectrum aignment operation
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("n_hits"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="n_hits")> Public Property n_hits As UInteger
    <DatabaseField("forward"), NotNull, DataType(MySqlDbType.Double), Column(Name:="forward")> Public Property forward As Double
    <DatabaseField("reverse"), NotNull, DataType(MySqlDbType.Double), Column(Name:="reverse")> Public Property reverse As Double
    <DatabaseField("jaccard"), NotNull, DataType(MySqlDbType.Double), Column(Name:="jaccard")> Public Property jaccard As Double
    <DatabaseField("entropy"), NotNull, DataType(MySqlDbType.Double), Column(Name:="entropy")> Public Property entropy As Double
''' <summary>
''' t-test p-value of [forward,reverse,jaccard,entropy]
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("p_value"), NotNull, DataType(MySqlDbType.Double), Column(Name:="p_value")> Public Property p_value As Double
''' <summary>
''' consensus ions m/z value, network byte order data stream in base64 string
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("consensus"), DataType(MySqlDbType.Text), Column(Name:="consensus")> Public Property consensus As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `cluster_data` (`cluster_id`, `model_id`, `metadata_id`, `spectral_id`, `score`, `n_hits`, `forward`, `reverse`, `jaccard`, `entropy`, `p_value`, `consensus`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `cluster_data` (`id`, `cluster_id`, `model_id`, `metadata_id`, `spectral_id`, `score`, `n_hits`, `forward`, `reverse`, `jaccard`, `entropy`, `p_value`, `consensus`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `cluster_data` (`cluster_id`, `model_id`, `metadata_id`, `spectral_id`, `score`, `n_hits`, `forward`, `reverse`, `jaccard`, `entropy`, `p_value`, `consensus`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `cluster_data` (`id`, `cluster_id`, `model_id`, `metadata_id`, `spectral_id`, `score`, `n_hits`, `forward`, `reverse`, `jaccard`, `entropy`, `p_value`, `consensus`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `cluster_data` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `cluster_data` SET `id`='{0}', `cluster_id`='{1}', `model_id`='{2}', `metadata_id`='{3}', `spectral_id`='{4}', `score`='{5}', `n_hits`='{6}', `forward`='{7}', `reverse`='{8}', `jaccard`='{9}', `entropy`='{10}', `p_value`='{11}', `consensus`='{12}', `add_time`='{13}' WHERE `id` = '{14}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `cluster_data` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `cluster_data` (`id`, `cluster_id`, `model_id`, `metadata_id`, `spectral_id`, `score`, `n_hits`, `forward`, `reverse`, `jaccard`, `entropy`, `p_value`, `consensus`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, cluster_id, model_id, metadata_id, spectral_id, score, n_hits, forward, reverse, jaccard, entropy, p_value, consensus, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `cluster_data` (`id`, `cluster_id`, `model_id`, `metadata_id`, `spectral_id`, `score`, `n_hits`, `forward`, `reverse`, `jaccard`, `entropy`, `p_value`, `consensus`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, cluster_id, model_id, metadata_id, spectral_id, score, n_hits, forward, reverse, jaccard, entropy, p_value, consensus, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, cluster_id, model_id, metadata_id, spectral_id, score, n_hits, forward, reverse, jaccard, entropy, p_value, consensus, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{cluster_id}', '{model_id}', '{metadata_id}', '{spectral_id}', '{score}', '{n_hits}', '{forward}', '{reverse}', '{jaccard}', '{entropy}', '{p_value}', '{consensus}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{cluster_id}', '{model_id}', '{metadata_id}', '{spectral_id}', '{score}', '{n_hits}', '{forward}', '{reverse}', '{jaccard}', '{entropy}', '{p_value}', '{consensus}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `cluster_data` (`id`, `cluster_id`, `model_id`, `metadata_id`, `spectral_id`, `score`, `n_hits`, `forward`, `reverse`, `jaccard`, `entropy`, `p_value`, `consensus`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, cluster_id, model_id, metadata_id, spectral_id, score, n_hits, forward, reverse, jaccard, entropy, p_value, consensus, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `cluster_data` (`id`, `cluster_id`, `model_id`, `metadata_id`, `spectral_id`, `score`, `n_hits`, `forward`, `reverse`, `jaccard`, `entropy`, `p_value`, `consensus`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, cluster_id, model_id, metadata_id, spectral_id, score, n_hits, forward, reverse, jaccard, entropy, p_value, consensus, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, cluster_id, model_id, metadata_id, spectral_id, score, n_hits, forward, reverse, jaccard, entropy, p_value, consensus, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `cluster_data` SET `id`='{0}', `cluster_id`='{1}', `model_id`='{2}', `metadata_id`='{3}', `spectral_id`='{4}', `score`='{5}', `n_hits`='{6}', `forward`='{7}', `reverse`='{8}', `jaccard`='{9}', `entropy`='{10}', `p_value`='{11}', `consensus`='{12}', `add_time`='{13}' WHERE `id` = '{14}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, cluster_id, model_id, metadata_id, spectral_id, score, n_hits, forward, reverse, jaccard, entropy, p_value, consensus, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As cluster_data
                         Return DirectCast(MyClass.MemberwiseClone, cluster_data)
                     End Function
End Class


End Namespace
