#Region "Microsoft.VisualBasic::5e43a7b72b994d59dbbcc803524a0257, Rscript\Library\mzkit_hpc\src\MoleculeTree\models\molecules.vb"

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

    '   Total Lines: 196
    '    Code Lines: 87 (44.39%)
    ' Comment Lines: 87 (44.39%)
    '    - Xml Docs: 96.55%
    ' 
    '   Blank Lines: 22 (11.22%)
    '     File Size: 9.12 KB


    ' Class molecules
    ' 
    '     Properties: add_time, db_xref, exact_mass, formula, id
    '                 name, smiles
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

REM  Dump @5/28/2025 02:55:30 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace treeModel

''' <summary>
''' ```SQL
''' 
''' --
''' 
''' DROP TABLE IF EXISTS `molecules`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `molecules` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `db_xref` varchar(255) NOT NULL COMMENT 'the unique reference id of current molecule from the external database',
'''   `name` varchar(2048) NOT NULL COMMENT 'common name of the metabolite',
'''   `formula` varchar(64) NOT NULL COMMENT 'formula string of the molecule',
'''   `exact_mass` double unsigned NOT NULL DEFAULT '0',
'''   `smiles` longtext NOT NULL COMMENT 'a unique representive canonical smiles structure data',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   UNIQUE KEY `db_xref_UNIQUE` (`db_xref`),
'''   KEY `find_by_xref` (`db_xref`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=223710 DEFAULT CHARSET=utf8mb3 COMMENT='cache pool of external molecule data set';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("molecules", Database:="molecule_tree", SchemaSQL:="
CREATE TABLE `molecules` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(255) NOT NULL COMMENT 'the unique reference id of current molecule from the external database',
  `name` varchar(2048) NOT NULL COMMENT 'common name of the metabolite',
  `formula` varchar(64) NOT NULL COMMENT 'formula string of the molecule',
  `exact_mass` double unsigned NOT NULL DEFAULT '0',
  `smiles` longtext NOT NULL COMMENT 'a unique representive canonical smiles structure data',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `db_xref_UNIQUE` (`db_xref`),
  KEY `find_by_xref` (`db_xref`)
) ENGINE=InnoDB AUTO_INCREMENT=223710 DEFAULT CHARSET=utf8mb3 COMMENT='cache pool of external molecule data set';")>
Public Class molecules: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the unique reference id of current molecule from the external database
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("db_xref"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="db_xref")> Public Property db_xref As String
''' <summary>
''' common name of the metabolite
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "2048"), Column(Name:="name")> Public Property name As String
''' <summary>
''' formula string of the molecule
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("formula"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="formula")> Public Property formula As String
    <DatabaseField("exact_mass"), NotNull, DataType(MySqlDbType.Double), Column(Name:="exact_mass")> Public Property exact_mass As Double
''' <summary>
''' a unique representive canonical smiles structure data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("smiles"), NotNull, DataType(MySqlDbType.Text), Column(Name:="smiles")> Public Property smiles As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `molecules` (`db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `molecules` (`db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `molecules` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `molecules` SET `id`='{0}', `db_xref`='{1}', `name`='{2}', `formula`='{3}', `exact_mass`='{4}', `smiles`='{5}', `add_time`='{6}' WHERE `id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `molecules` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{db_xref}', '{name}', '{formula}', '{exact_mass}', '{smiles}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{db_xref}', '{name}', '{formula}', '{exact_mass}', '{smiles}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `molecules` SET `id`='{0}', `db_xref`='{1}', `name`='{2}', `formula`='{3}', `exact_mass`='{4}', `smiles`='{5}', `add_time`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As molecules
                         Return DirectCast(MyClass.MemberwiseClone, molecules)
                     End Function
End Class


End Namespace
