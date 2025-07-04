#Region "Microsoft.VisualBasic::410aaffd6f3034c48c3252fec59bd4cf, Rscript\Library\mzkit_hpc\src\spectrumPool\mysql\models\metadata.vb"

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

    '   Total Lines: 242
    '    Code Lines: 125 (51.65%)
    ' Comment Lines: 95 (39.26%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 22 (9.09%)
    '     File Size: 15.91 KB


    ' Class metadata
    ' 
    '     Properties: add_time, adducts, biosample, cluster_id, filename
    '                 formula, hashcode, id, instrument, intensity
    '                 model_id, mz, name, organism, project
    '                 project_id, rawfile, rt, spectral_id, xref_id
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
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("metadata", Database:="sample_pool", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `metadata` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `hashcode` VARCHAR(32) NOT NULL,
  `mz` DOUBLE NOT NULL DEFAULT '-1' COMMENT 'ms2 spectrum its precursor m/z',
  `rt` DOUBLE NOT NULL DEFAULT '0' COMMENT 'rt of this ms2 spectrum, in time data unit seconds',
  `intensity` DOUBLE NOT NULL DEFAULT '0' COMMENT 'intensity of the ms2 spectrum precursor',
  `filename` VARCHAR(255) NOT NULL,
  `cluster_id` INT UNSIGNED NOT NULL,
  `rawfile` INT UNSIGNED NOT NULL COMMENT 'the raw data file reference id',
  `spectral_id` INT UNSIGNED NOT NULL,
  `model_id` INT UNSIGNED NOT NULL,
  `project_id` INT UNSIGNED NOT NULL DEFAULT 0,
  `project` VARCHAR(255) NOT NULL DEFAULT 'NA',
  `biosample` VARCHAR(64) NOT NULL,
  `organism` VARCHAR(128) NOT NULL,
  `xref_id` VARCHAR(255) NOT NULL COMMENT 'the metabolite annotation reference id to the external metabolite informatyion database, example as biodeep_000000001  or unknown conserved',
  `name` VARCHAR(4096) NOT NULL COMMENT 'the metabolite name',
  `formula` VARCHAR(64) NOT NULL COMMENT 'formula string of the this metabolite, empty for unknown conserved',
  `adducts` VARCHAR(32) NOT NULL COMMENT 'adducts of the metabolite annotation',
  `instrument` VARCHAR(255) NOT NULL DEFAULT 'Thermo Scientific Q Exactive' COMMENT 'instrument name of the raw data file where it generates',
  `add_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `idmetadata_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `project_index` (`project_id` ASC) INVISIBLE,
  INDEX `project_index2` (`project` ASC) INVISIBLE,
  INDEX `model_index` (`model_id` ASC) VISIBLE,
  INDEX `metadata_query` (`hashcode` ASC, `cluster_id` ASC, `model_id` ASC) INVISIBLE,
  INDEX `check_duplicated` (`hashcode` ASC, `filename` ASC, `cluster_id` ASC, `model_id` ASC, `project` ASC, `xref_id` ASC) VISIBLE,
  INDEX `biosample_stats` (`organism` ASC, `biosample` ASC) INVISIBLE,
  INDEX `cluster_reference_index` (`cluster_id` ASC) VISIBLE,
  INDEX `metadata_query_index` (`id` ASC, `model_id` ASC, `cluster_id` ASC) VISIBLE,
  INDEX `mass_index` (`mz` ASC) VISIBLE,
  INDEX `time_index` (`rt` ASC) VISIBLE,
  INDEX `spectrum_reference_idx` (`spectral_id` ASC) VISIBLE,
  INDEX `file_source_idx` (`rawfile` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;
")>
Public Class metadata: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
''' <summary>
''' ms2 spectrum its precursor m/z
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("mz"), NotNull, DataType(MySqlDbType.Double), Column(Name:="mz")> Public Property mz As Double
''' <summary>
''' rt of this ms2 spectrum, in time data unit seconds
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("rt"), NotNull, DataType(MySqlDbType.Double), Column(Name:="rt")> Public Property rt As Double
''' <summary>
''' intensity of the ms2 spectrum precursor
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("intensity"), NotNull, DataType(MySqlDbType.Double), Column(Name:="intensity")> Public Property intensity As Double
    <DatabaseField("filename"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="filename")> Public Property filename As String
    <DatabaseField("cluster_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="cluster_id")> Public Property cluster_id As UInteger
''' <summary>
''' the raw data file reference id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("rawfile"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="rawfile")> Public Property rawfile As UInteger
    <DatabaseField("spectral_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="spectral_id")> Public Property spectral_id As UInteger
    <DatabaseField("model_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="model_id")> Public Property model_id As UInteger
    <DatabaseField("project_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="project_id")> Public Property project_id As UInteger
    <DatabaseField("project"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="project")> Public Property project As String
    <DatabaseField("biosample"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="biosample")> Public Property biosample As String
    <DatabaseField("organism"), NotNull, DataType(MySqlDbType.VarChar, "128"), Column(Name:="organism")> Public Property organism As String
''' <summary>
''' the metabolite annotation reference id to the external metabolite informatyion database, example as biodeep_000000001  or unknown conserved
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("xref_id"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="xref_id")> Public Property xref_id As String
''' <summary>
''' the metabolite name
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "4096"), Column(Name:="name")> Public Property name As String
''' <summary>
''' formula string of the this metabolite, empty for unknown conserved
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("formula"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="formula")> Public Property formula As String
''' <summary>
''' adducts of the metabolite annotation
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("adducts"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="adducts")> Public Property adducts As String
''' <summary>
''' instrument name of the raw data file where it generates
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("instrument"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="instrument")> Public Property instrument As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `metadata` (`hashcode`, `mz`, `rt`, `intensity`, `filename`, `cluster_id`, `rawfile`, `spectral_id`, `model_id`, `project_id`, `project`, `biosample`, `organism`, `xref_id`, `name`, `formula`, `adducts`, `instrument`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `metadata` (`id`, `hashcode`, `mz`, `rt`, `intensity`, `filename`, `cluster_id`, `rawfile`, `spectral_id`, `model_id`, `project_id`, `project`, `biosample`, `organism`, `xref_id`, `name`, `formula`, `adducts`, `instrument`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `metadata` (`hashcode`, `mz`, `rt`, `intensity`, `filename`, `cluster_id`, `rawfile`, `spectral_id`, `model_id`, `project_id`, `project`, `biosample`, `organism`, `xref_id`, `name`, `formula`, `adducts`, `instrument`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `metadata` (`id`, `hashcode`, `mz`, `rt`, `intensity`, `filename`, `cluster_id`, `rawfile`, `spectral_id`, `model_id`, `project_id`, `project`, `biosample`, `organism`, `xref_id`, `name`, `formula`, `adducts`, `instrument`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `metadata` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `metadata` SET `id`='{0}', `hashcode`='{1}', `mz`='{2}', `rt`='{3}', `intensity`='{4}', `filename`='{5}', `cluster_id`='{6}', `rawfile`='{7}', `spectral_id`='{8}', `model_id`='{9}', `project_id`='{10}', `project`='{11}', `biosample`='{12}', `organism`='{13}', `xref_id`='{14}', `name`='{15}', `formula`='{16}', `adducts`='{17}', `instrument`='{18}', `add_time`='{19}' WHERE `id` = '{20}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `metadata` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `metadata` (`id`, `hashcode`, `mz`, `rt`, `intensity`, `filename`, `cluster_id`, `rawfile`, `spectral_id`, `model_id`, `project_id`, `project`, `biosample`, `organism`, `xref_id`, `name`, `formula`, `adducts`, `instrument`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, hashcode, mz, rt, intensity, filename, cluster_id, rawfile, spectral_id, model_id, project_id, project, biosample, organism, xref_id, name, formula, adducts, instrument, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `metadata` (`id`, `hashcode`, `mz`, `rt`, `intensity`, `filename`, `cluster_id`, `rawfile`, `spectral_id`, `model_id`, `project_id`, `project`, `biosample`, `organism`, `xref_id`, `name`, `formula`, `adducts`, `instrument`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, hashcode, mz, rt, intensity, filename, cluster_id, rawfile, spectral_id, model_id, project_id, project, biosample, organism, xref_id, name, formula, adducts, instrument, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, hashcode, mz, rt, intensity, filename, cluster_id, rawfile, spectral_id, model_id, project_id, project, biosample, organism, xref_id, name, formula, adducts, instrument, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{hashcode}', '{mz}', '{rt}', '{intensity}', '{filename}', '{cluster_id}', '{rawfile}', '{spectral_id}', '{model_id}', '{project_id}', '{project}', '{biosample}', '{organism}', '{xref_id}', '{name}', '{formula}', '{adducts}', '{instrument}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{hashcode}', '{mz}', '{rt}', '{intensity}', '{filename}', '{cluster_id}', '{rawfile}', '{spectral_id}', '{model_id}', '{project_id}', '{project}', '{biosample}', '{organism}', '{xref_id}', '{name}', '{formula}', '{adducts}', '{instrument}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `metadata` (`id`, `hashcode`, `mz`, `rt`, `intensity`, `filename`, `cluster_id`, `rawfile`, `spectral_id`, `model_id`, `project_id`, `project`, `biosample`, `organism`, `xref_id`, `name`, `formula`, `adducts`, `instrument`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, hashcode, mz, rt, intensity, filename, cluster_id, rawfile, spectral_id, model_id, project_id, project, biosample, organism, xref_id, name, formula, adducts, instrument, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `metadata` (`id`, `hashcode`, `mz`, `rt`, `intensity`, `filename`, `cluster_id`, `rawfile`, `spectral_id`, `model_id`, `project_id`, `project`, `biosample`, `organism`, `xref_id`, `name`, `formula`, `adducts`, `instrument`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, hashcode, mz, rt, intensity, filename, cluster_id, rawfile, spectral_id, model_id, project_id, project, biosample, organism, xref_id, name, formula, adducts, instrument, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, hashcode, mz, rt, intensity, filename, cluster_id, rawfile, spectral_id, model_id, project_id, project, biosample, organism, xref_id, name, formula, adducts, instrument, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `metadata` SET `id`='{0}', `hashcode`='{1}', `mz`='{2}', `rt`='{3}', `intensity`='{4}', `filename`='{5}', `cluster_id`='{6}', `rawfile`='{7}', `spectral_id`='{8}', `model_id`='{9}', `project_id`='{10}', `project`='{11}', `biosample`='{12}', `organism`='{13}', `xref_id`='{14}', `name`='{15}', `formula`='{16}', `adducts`='{17}', `instrument`='{18}', `add_time`='{19}' WHERE `id` = '{20}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, hashcode, mz, rt, intensity, filename, cluster_id, rawfile, spectral_id, model_id, project_id, project, biosample, organism, xref_id, name, formula, adducts, instrument, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As metadata
                         Return DirectCast(MyClass.MemberwiseClone, metadata)
                     End Function
End Class


End Namespace
