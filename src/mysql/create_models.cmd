@echo off

set reflector="\graphQL\src\mysqli\App\Reflector.exe"
set source="./molecule_tree.sql"
set out="../MoleculeTree/models"

%reflector% --reflects /sql %source% -o %out% /namespace treeModel --language visualbasic /split 