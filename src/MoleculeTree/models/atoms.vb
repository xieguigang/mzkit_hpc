#Region "Microsoft.VisualBasic::1f1b8d562e68683568e511088641c519, Rscript\Library\mzkit_hpc\src\MoleculeTree\models\atoms.vb"

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
    '    Code Lines: 91 (39.22%)
    ' Comment Lines: 119 (51.29%)
    '    - Xml Docs: 97.48%
    ' 
    '   Blank Lines: 22 (9.48%)
    '     File Size: 11.61 KB


    ' Class atoms
    ' 
    '     Properties: add_time, aromatic, atom_group, charge, element
    '                 hydrogen, id, note, unique_id
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

REM  Dump @12/5/2024 10:32:30 AM


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
''' DROP TABLE IF EXISTS `atoms`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `atoms` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'the unique reference id of current atom group, in integer id representative',
'''   `unique_id` varchar(64) NOT NULL COMMENT 'the unique reference id of current atom group, in character representative',
'''   `atom_group` varchar(45) NOT NULL COMMENT 'the atom group name, could be duplicated',
'''   `element` varchar(8) NOT NULL COMMENT 'the base element atom of current group data',
'''   `aromatic` int unsigned NOT NULL DEFAULT '0' COMMENT 'on an aromatic ring? 1 means true',
'''   `hydrogen` int unsigned NOT NULL DEFAULT '0' COMMENT 'The number of the hydrogen of current atom group it has',
'''   `charge` int NOT NULL DEFAULT '0' COMMENT 'the ion charge value of current atom group, could be a positive/negative integer value',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'create time of this atom group',
'''   `note` longtext COMMENT 'description about this atom group data',
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   UNIQUE KEY `unique_id_UNIQUE` (`unique_id`) /*!80000 INVISIBLE */,
'''   KEY `find_reference` (`unique_id`)
''' ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='atom group information data';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("atoms", Database:="molecule_tree", SchemaSQL:="
CREATE TABLE `atoms` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'the unique reference id of current atom group, in integer id representative',
  `unique_id` varchar(64) NOT NULL COMMENT 'the unique reference id of current atom group, in character representative',
  `atom_group` varchar(45) NOT NULL COMMENT 'the atom group name, could be duplicated',
  `element` varchar(8) NOT NULL COMMENT 'the base element atom of current group data',
  `aromatic` int unsigned NOT NULL DEFAULT '0' COMMENT 'on an aromatic ring? 1 means true',
  `hydrogen` int unsigned NOT NULL DEFAULT '0' COMMENT 'The number of the hydrogen of current atom group it has',
  `charge` int NOT NULL DEFAULT '0' COMMENT 'the ion charge value of current atom group, could be a positive/negative integer value',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'create time of this atom group',
  `note` longtext COMMENT 'description about this atom group data',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_id_UNIQUE` (`unique_id`) /*!80000 INVISIBLE */,
  KEY `find_reference` (`unique_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='atom group information data';")>
Public Class atoms: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' the unique reference id of current atom group, in integer id representative
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the unique reference id of current atom group, in character representative
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("unique_id"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="unique_id")> Public Property unique_id As String
''' <summary>
''' the atom group name, could be duplicated
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("atom_group"), NotNull, DataType(MySqlDbType.VarChar, "45"), Column(Name:="atom_group")> Public Property atom_group As String
''' <summary>
''' the base element atom of current group data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("element"), NotNull, DataType(MySqlDbType.VarChar, "8"), Column(Name:="element")> Public Property element As String
''' <summary>
''' on an aromatic ring? 1 means true
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("aromatic"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="aromatic")> Public Property aromatic As UInteger
''' <summary>
''' The number of the hydrogen of current atom group it has
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hydrogen"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="hydrogen")> Public Property hydrogen As UInteger
''' <summary>
''' the ion charge value of current atom group, could be a positive/negative integer value
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("charge"), NotNull, DataType(MySqlDbType.Int32, "11"), Column(Name:="charge")> Public Property charge As Integer
''' <summary>
''' create time of this atom group
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
''' <summary>
''' description about this atom group data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `atoms` (`unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `atoms` (`unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `atoms` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `atoms` SET `id`='{0}', `unique_id`='{1}', `atom_group`='{2}', `element`='{3}', `aromatic`='{4}', `hydrogen`='{5}', `charge`='{6}', `add_time`='{7}', `note`='{8}' WHERE `id` = '{9}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `atoms` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{unique_id}', '{atom_group}', '{element}', '{aromatic}', '{hydrogen}', '{charge}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{unique_id}', '{atom_group}', '{element}', '{aromatic}', '{hydrogen}', '{charge}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `atoms` SET `id`='{0}', `unique_id`='{1}', `atom_group`='{2}', `element`='{3}', `aromatic`='{4}', `hydrogen`='{5}', `charge`='{6}', `add_time`='{7}', `note`='{8}' WHERE `id` = '{9}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As atoms
                         Return DirectCast(MyClass.MemberwiseClone, atoms)
                     End Function
End Class


End Namespace
