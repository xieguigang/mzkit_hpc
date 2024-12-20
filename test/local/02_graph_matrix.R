require(mzkit_hpc);
require(graphQL);

imports "molecule_tree" from "hpc";

let tree = mysql::open(   
        user_name = "root",
        password = 123456,
        dbname = "molecule_tree",
        host  = "192.168.3.233",
        port = 3306
    );

tree |> molecule_tree::update_matrix();