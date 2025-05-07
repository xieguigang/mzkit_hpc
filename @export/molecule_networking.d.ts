// export R# package module type define for javascript/typescript language
//
//    imports "molecule_networking" from "hpc";
//
// ref=hpc.MoleculeNetworking@hpc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * make clustering and networking of the lc-ms ms2 spectrum data
 * 
*/
declare namespace molecule_networking {
   /**
    * create a new spectrum clustering data pool
    * 
    * 
     * @param level -
     * 
     * + default value Is ``0.9``.
     * @param split hex, max=15
     * 
     * + default value Is ``9``.
     * @param name 
     * + default value Is ``'no_named'``.
     * @param desc 
     * + default value Is ``'no_information'``.
   */
   function createMysqlPool(repo: object, level?: number, split?: object, name?: string, desc?: string): object;
   /**
     * @param group default value Is ``null``.
   */
   function file_info(repo: object, filepath: string, group?: string): object;
   /**
     * @param organism default value Is ``'Unknown'``.
     * @param bio_sample default value Is ``'Unknown'``.
     * @param repo_dir default value Is ``''``.
   */
   function group_info(repo: object, group: string, organism?: string, bio_sample?: string, repo_dir?: string): object;
   /**
    * open the spectrum pool from a given resource link
    * 
    * 
     * @param model_id the model id, this parameter works for open the model in the cloud service
     * 
     * + default value Is ``null``.
     * @param score_overrides WARNING: this optional parameter will overrides the mode score 
     *  level when this parameter has a positive numeric value in 
     *  range ``(0,1]``. it is dangers to overrides the score parameter
     *  in the exists model.
     * 
     * + default value Is ``null``.
     * @param env 
     * + default value Is ``null``.
   */
   function openMysqlPool(repo: object, model_id?: string, score_overrides?: object, env?: object): object;
   /**
     * @param name default value Is ``null``.
     * @param desc default value Is ``null``.
   */
   function project_context(repo: object, project_id: string, name?: string, desc?: string): object;
}
