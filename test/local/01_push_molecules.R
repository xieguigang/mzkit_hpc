require(mzkit_hpc);
require(graphQL);

imports "molecule_tree" from "hpc";

let data = read.csv("G:\metaboanalyst\data\smiles.csv",row.names = NULL);
let tree = mysql::open(   
        user_name = "root",
        password = 123456,
        dbname = "molecule_tree",
        host  = "192.168.3.233",
        port = 3306
    );
let smiles_str ="";

print(data);

for(let met in tqdm(as.list(data, byrow=TRUE))) {
    smiles_str = met$smiles;
    met = new metabo_data(
        Id = met$ID,
        CommonName = met$name,
        ExactMass = met$exact_mass,
        Formula = met$formula
    );

    tree |> add_molecule(
        met, smiles = smiles_str
    );
}