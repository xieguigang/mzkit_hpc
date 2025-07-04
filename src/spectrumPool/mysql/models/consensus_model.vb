#Region "Microsoft.VisualBasic::62b54c7b39efb0b34f6f8a92616f0ef7, Rscript\Library\mzkit_hpc\src\spectrumPool\mysql\models\consensus_model.vb"

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

    '   Total Lines: 171
    '    Code Lines: 89 (52.05%)
    ' Comment Lines: 60 (35.09%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 22 (12.87%)
    '     File Size: 8.61 KB


    ' Class consensus_model
    ' 
    '     Properties: add_time, consensus_cutoff, id, model_id, umap_dimension
    '                 umap_neighbors, umap_others
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
''' model and parameters for create consensus analysis result
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("consensus_model", Database:="sample_pool", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `consensus_model` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'the parameter id',
  `model_id` INT UNSIGNED NOT NULL,
  `consensus_cutoff` DOUBLE UNSIGNED NOT NULL DEFAULT '0.3' COMMENT 'cutoff value for test two spectrum is identical or not',
  `umap_dimension` INT UNSIGNED NOT NULL DEFAULT '10',
  `umap_neighbors` INT UNSIGNED NOT NULL DEFAULT '15',
  `umap_others` JSON NOT NULL COMMENT 'other umap parameters for create embedding',
  `add_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `model_info_idx` (`model_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3
COMMENT = 'model and parameters for create consensus analysis result';
")>
Public Class consensus_model: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' the parameter id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("model_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="model_id")> Public Property model_id As UInteger
''' <summary>
''' cutoff value for test two spectrum is identical or not
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("consensus_cutoff"), NotNull, DataType(MySqlDbType.Double), Column(Name:="consensus_cutoff")> Public Property consensus_cutoff As Double
    <DatabaseField("umap_dimension"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="umap_dimension")> Public Property umap_dimension As UInteger
    <DatabaseField("umap_neighbors"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="umap_neighbors")> Public Property umap_neighbors As UInteger
''' <summary>
''' other umap parameters for create embedding
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("umap_others"), NotNull, DataType(MySqlDbType.Text), Column(Name:="umap_others")> Public Property umap_others As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `consensus_model` (`model_id`, `consensus_cutoff`, `umap_dimension`, `umap_neighbors`, `umap_others`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `consensus_model` (`id`, `model_id`, `consensus_cutoff`, `umap_dimension`, `umap_neighbors`, `umap_others`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `consensus_model` (`model_id`, `consensus_cutoff`, `umap_dimension`, `umap_neighbors`, `umap_others`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `consensus_model` (`id`, `model_id`, `consensus_cutoff`, `umap_dimension`, `umap_neighbors`, `umap_others`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `consensus_model` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `consensus_model` SET `id`='{0}', `model_id`='{1}', `consensus_cutoff`='{2}', `umap_dimension`='{3}', `umap_neighbors`='{4}', `umap_others`='{5}', `add_time`='{6}' WHERE `id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `consensus_model` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `consensus_model` (`id`, `model_id`, `consensus_cutoff`, `umap_dimension`, `umap_neighbors`, `umap_others`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, model_id, consensus_cutoff, umap_dimension, umap_neighbors, umap_others, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `consensus_model` (`id`, `model_id`, `consensus_cutoff`, `umap_dimension`, `umap_neighbors`, `umap_others`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, model_id, consensus_cutoff, umap_dimension, umap_neighbors, umap_others, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, model_id, consensus_cutoff, umap_dimension, umap_neighbors, umap_others, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{model_id}', '{consensus_cutoff}', '{umap_dimension}', '{umap_neighbors}', '{umap_others}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{model_id}', '{consensus_cutoff}', '{umap_dimension}', '{umap_neighbors}', '{umap_others}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `consensus_model` (`id`, `model_id`, `consensus_cutoff`, `umap_dimension`, `umap_neighbors`, `umap_others`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, model_id, consensus_cutoff, umap_dimension, umap_neighbors, umap_others, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `consensus_model` (`id`, `model_id`, `consensus_cutoff`, `umap_dimension`, `umap_neighbors`, `umap_others`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, model_id, consensus_cutoff, umap_dimension, umap_neighbors, umap_others, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, model_id, consensus_cutoff, umap_dimension, umap_neighbors, umap_others, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `consensus_model` SET `id`='{0}', `model_id`='{1}', `consensus_cutoff`='{2}', `umap_dimension`='{3}', `umap_neighbors`='{4}', `umap_others`='{5}', `add_time`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, model_id, consensus_cutoff, umap_dimension, umap_neighbors, umap_others, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As consensus_model
                         Return DirectCast(MyClass.MemberwiseClone, consensus_model)
                     End Function
End Class


End Namespace
