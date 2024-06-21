// export R# package module type define for javascript/typescript language
//
//    imports "MsImaging" from "hpc";
//
// ref=hpc.MsImaging@hpc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * MS-imaging rawdata processing in HPC parallel
 * 
*/
declare namespace MsImaging {
   /**
    * run measure of the ion features in IPC parallel for a huge ms-imaging rawdata matrix
    * 
    * 
     * @param x -
     * @param grid_size -
     * 
     * + default value Is ``5``.
   */
   function MSI_ionStat_parallel(x: object, grid_size?: object): object;
   /**
    * run measure of the ion features in IPC parallel for a huge single cells rawdata matrix
    * 
    * 
     * @param x -
   */
   function SCMs_ionStat_parallel(x: object): object;
}
