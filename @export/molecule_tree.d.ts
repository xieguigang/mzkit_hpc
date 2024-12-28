// export R# package module type define for javascript/typescript language
//
//    imports "molecule_tree" from "hpc";
//
// ref=hpc.MoleculeCluster@hpc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * Create molecule tree via strucutre clustering for run unknown spectrum feature annotation
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
    * get molecule graph matrix data
    * 
    * 
     * @param tree -
     * @param db_xrefs -
     * @param prefix -
     * 
     * + default value Is ``null``.
     * @param scalar the function returns a scalar molecule result: a numeric vector if not base64, or the raw base64 string of the matrix.
     * 
     * + default value Is ``false``.
     * @param base64 -
     * 
     * + default value Is ``false``.
   */
   function fetch_matrix(tree: object, db_xrefs: any, prefix?: string, scalar?: boolean, base64?: boolean): any;
   /**
    * Download the molecule tree graph from the database
    * 
    * 
     * @param tree -
     * @param model -
   */
   function fetch_tree(tree: object, model: string): object;
   /**
     * @param cluster_cutoff default value Is ``null``.
     * @param right_cutoff default value Is ``null``.
   */
   function make_clusterTree(tree: object, model: string, cluster_cutoff?: object, right_cutoff?: object): ;
   /**
    * get a set of the molecule information in a given model
    * 
    * 
     * @param tree -
     * @param model the name reference to a specific model
     * @param env 
     * + default value Is ``null``.
   */
   function molecule_set(tree: object, model: string, env?: object): any;
   /**
     * @param page_size default value Is ``100``.
     * @param fast_check default value Is ``false``.
   */
   function update_matrix(tree: object, page_size?: object, fast_check?: boolean): ;
}
