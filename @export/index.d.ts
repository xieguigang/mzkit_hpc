// export R# source type define for javascript/typescript language
//
// package_source=mzkit_hpc

declare namespace mzkit_hpc {
   module _ {
      /**
      */
      function onLoad(): object;
   }
   /**
     * @param outputdir default value Is ``./``.
     * @param mzdiff default value Is ``0.005``.
     * @param peak.width default value Is ``[3, 90]``.
   */
   function deconvolution(rawdata: any, outputdir?: any, mzdiff?: any, peak.width?: any): object;
   /**
     * @param instrument_name default value Is ``Thermo Scientific Q Exactive``.
     * @param model_name default value Is ``Call "list"("positive" <- "xxx_pos", "negative" <- "xxx_neg")``.
     * @param repo default value Is ``Call "open_mnlink"("user" <- "xxx",
     *       "passwd" <- "xxx",
     *       "host" <- "127.0.0.1",
     *       "port" <- 3306)``.
   */
   function imports_metabolights(sample_metadata: any, instrument_name?: any, model_name?: any, repo?: any): object;
   /**
     * @param mzdiff default value Is ``0.005``.
     * @param peak.width default value Is ``[3, 90]``.
   */
   function ms1_peaktable(files: any, mzbins: any, mzdiff?: any, peak.width?: any): object;
   /**
     * @param host default value Is ``127.0.0.1``.
     * @param port default value Is ``3306``.
   */
   function open_mnlink(user: any, passwd: any, host?: any, port?: any): object;
}
