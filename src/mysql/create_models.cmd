@echo off

set reflector="\graphQL\src\mysqli\App\Reflector.exe"
set source="./molecule_tree.sql"
set out="../MoleculeTree/models"

%reflector% --reflects /sql %source% -o %out% /namespace treeModel --language visualbasic /split 

set source="./sample_pool.sql"
set out="../spectrumPool/mysql/models"

%reflector% --reflects /sql %source% -o %out% /namespace clusterModels --language visualbasic /split 