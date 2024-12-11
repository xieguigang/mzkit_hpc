# molecule_tree

Create molecule tree via strucutre clustering for run unknown spectrum feature annotation
> steps for build molecule tree:
>  
>  1. push molecule structre data
>  2. parse structure data as graph
>  3. push many strcutre data
>  4. update graph matrix data
>  5. build molecule tree finally

+ [add_molecule](molecule_tree/add_molecule.1) add molecule model data into database pool
+ [update_matrix](molecule_tree/update_matrix.1) 
+ [make_clusterTree](molecule_tree/make_clusterTree.1) 
+ [fetch_tree](molecule_tree/fetch_tree.1) Download the molecule tree graph from the database
+ [molecule_set](molecule_tree/molecule_set.1) get a set of the molecule information in a given model
