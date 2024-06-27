// export R# package module type define for javascript/typescript language
//
//    imports "deconvolution" from "hpc";
//
// ref=hpc.deconvolution@hpc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace deconvolution {
   /**
    * Make peak group alignments and export peaktable
    * 
    * 
     * @param peak_groups -
     * @param features_mz -
     * @param tolerance 
     * + default value Is ``'da:0.005'``.
     * @param peak_width 
     * + default value Is ``'3,15'``.
     * @param baseline 
     * + default value Is ``0.65``.
     * @param joint 
     * + default value Is ``false``.
     * @param dtw 
     * + default value Is ``false``.
     * @param env 
     * + default value Is ``null``.
   */
   function peak_alignments(peak_groups: object, features_mz: number, tolerance?: any, peak_width?: any, baseline?: number, joint?: boolean, dtw?: boolean, env?: object): any;
}
