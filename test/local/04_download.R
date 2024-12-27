require(mzkit_hpc);
require(graphQL);
require(igraph);

imports "molecule_tree" from "hpc";

let tree = mysql::open(   
    user_name = "root",
    password = 123456,
    dbname = "molecule_tree",
    host  = "192.168.3.233",
    port = 3306
);
let graph = tree |> fetch_tree(model = "metabolic-tree");

igraph::save.network(graph, file = file.path(@dir,"treeModel_20241228"), properties="*");