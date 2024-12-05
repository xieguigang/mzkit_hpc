// export R# package module type define for javascript/typescript language
//
//    imports "molecule_tree" from "hpc";
//
// ref=hpc.MoleculeCluster@hpc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
 * > steps for build molecule tree:
 * >  
 * >  1. push molecule structre data
 * >  2. parse structure data as graph
 * >  3. push many strcutre data
 * >  4. update graph matrix data
 * >  5. build molecule tree finally
*/
declare namespace molecule_tree {
   /**
    * add molecule model data into database pool
    * 
    * 
     * @param tree -
     * @param meta the brief metabolite annotation information for make cache
     * @param smiles molecule structure data
   */
   function add_molecule(tree: object, meta: object, smiles: any): ;
   /**
   */
   function fetch_tree(tree: object, model: string): object;
   /**
     * @param cluster_cutoff default value Is ``null``.
     * @param right_cutoff default value Is ``null``.
   */
   function make_clusterTree(tree: object, model: string, cluster_cutoff?: object, right_cutoff?: object): ;
   /**
     * @param page_size default value Is ``100``.
   */
   function update_matrix(tree: object, page_size?: object): ;
}
