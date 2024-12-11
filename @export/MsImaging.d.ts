// export R# package module type define for javascript/typescript language
//
//    imports "MsImaging" from "hpc";
//
// ref=hpc.MsImaging@hpc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * MS-imaging rawdata processing helper in HPC parallel
 * 
*/
declare namespace MsImaging {
   /**
    * image processor for huge HE-stain bitmap file
    * 
    * 
     * @param file the file path to the HE-stain image file, should be processed as bitmap 
     *  file at first via image processing software like photoshop.
   */
   function HEstain_tissue_reader(file: string): any;
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
