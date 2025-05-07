#' Open a connection to the MySQL database for the spectrum data pool system
#' 
#' @description Establishes a connection to the MySQL database used for storing spectral data and 
#'              molecular networking information. Requires the graphQL package and MySQL driver.
#' 
#' @param user Character string specifying the MySQL username for authentication.
#' @param passwd Character string containing the password for the MySQL user.
#' @param host Character string specifying the host address of the MySQL server. 
#'             Default is "127.0.0.1" (localhost).
#' @param port Integer specifying the port number for the MySQL connection. 
#'            Default is 3306 (standard MySQL port).
#'
#' @return Returns a MySQL connection object (CLR object) that can be used for subsequent database 
#'         operations. The connection is configured to use the "sample_pool" database.
#'
#' @details This function loads required packages (graphQL and hpc) and initializes a connection 
#'          using the specified credentials. Ensure MySQL service is running at the target host/port.
#'
#' @examples
#' \dontrun{
#'   conn <- open_mnlink(user = "admin", passwd = "password", host = "db.server.com")
#' }
#'
#' @seealso \code{\link[graphQL]{mysql}} for MySQL connection management
const open_mnlink = function(user, passwd, host = "127.0.0.1", port = 3306) {
    require(graphQL);

    imports "mysql" from "graphR";
    imports "molecule_networking" from "hpc";

    mysql::open(
        user_name = user,
         password = passwd,
           dbname = "sample_pool",
             host = host,
             port = port
    );
}

#' Import MetaboLights project data into the spectrum data pool system
#' 
#' @description Processes MetaboLights study data including sample metadata and investigation files,
#'              then imports mass spectrometry data into specified clustering models in the database.
#' 
#' @param sample_metadata Character string specifying the file path to the sample metadata 
#'                        (e.g., s_*.txt from MetaboLights). Should contain biosample information.
#' @param investigation Character string specifying the file path to the MetaboLights investigation
#'                      file (i_Investigation.txt). Used to extract study-level metadata.
#' @param model_name Either a named list or character string specifying spectral clustering models:
#'                   * If a named list, must contain "positive" and "negative" elements specifying 
#'                     model names for each polarity mode
#'                   * If a character string, models will be auto-generated with "_POS" and "_NEG" 
#'                     suffixes for respective polarities
#'                   Default: list(positive = "xxx_pos", negative = "xxx_neg")
#' @param repo MySQL connection object created by \code{open_mnlink}. Default uses placeholder 
#'             credentials (replace for production use).
#' 
#' @return Invisibly returns NULL. Main effects include:
#'         * Uploads study metadata to MySQL database
#'         * Imports raw spectra into specified clustering models
#'         * Updates file metadata in the repository
#' 
#' @details Requires MetaboLights package for study file parsing and JSON package for data handling.
#'          Raw data files are expected in [investigation_dir]/FILES/[polarity]/[sample_name].raw.
#'          The function performs:
#'          1. Study metadata parsing and database registration
#'          2. Polarity-specific data insertion into clustering models
#'          3. Biosample-instrument associations
#' 
#' @section Parameter Specialization:
#' When using character input for \code{model_name}, the function automatically appends "_POS" and 
#' "_NEG" to create model names. For example, input "my_model" becomes list(positive = "my_model_POS", 
#' negative = "my_model_NEG").
#'
#' @examples
#' \dontrun{
#'   # Using explicit model list
#'   imports_metabolights(
#'     sample_metadata = "s_study.txt",
#'     investigation = "i_Investigation.txt",
#'     model_name = list(positive = "metab_pos", negative = "metab_neg"),
#'     repo = open_mnlink(user = "admin", passwd = "secret")
#'   )
#'   
#'   # Using auto-generated model names
#'   imports_metabolights(
#'     sample_metadata = "s_study.txt",
#'     investigation = "i_Investigation.txt",
#'     model_name = "global_model"
#'   )
#' }
#'
#' @seealso \code{\link{open_mnlink}} for connection creation, 
#'          \code{\link[MetaboLights]{MTBLSStudy}} for study parsing
const imports_metabolights = function(sample_metadata, investigation, 
                                      model_name = list(positive = "xxx_pos", negative = "xxx_neg"), 
                                      repo = open_mnlink(user = "xxx", passwd = "xxx", host = "127.0.0.1", port = 3306)) {
    require(MetaboLights);
    require(JSON);

    let metadata = MTBLSStudy::read.study_source(file = sample_metadata);
    let studyinfo = MTBLSStudy::read.study_metadata(file = investigation);
    let sampleinfo = as.data.frame(metadata); 

    print("view of the sample information:");
    print(sampleinfo);
    print("view of the project metadata:");
    print(`project_id: ${studyinfo[["Study Identifier"]]}`);
    print(`project_name: ${studyinfo[["Study Title"]]}`);
    print("project information & abstract:");
    print(studyinfo[["Study Description"]]);

    # save study project information into 
    # the mysql database system
    repo |> project_context(studyinfo[["Study Identifier"]], 
                            studyinfo[["Study Title"]], 
                            studyinfo[["Study Description"]]);   

    let insert_into = function(model_id, polarity) {
        let model = repo |> openMysqlPool(model_id = model_id);       

        for(let sample in as.list(sampleinfo, byrow = TRUE)) {
            let name = sample$name;
            let filepath = file.path(dirname(investigation),"FILES", polarity, `${name}.raw`);
            let rawdata = open.mzpack(filepath );
            
            repo  |> file_info(filepath);
            model |> addPool( 
                x = ms2_peaks(rawdata),
                biosample  = sample[["Characteristics[Organism part]"]],
                organism  = sample[["Characteristics[Organism]"]],
                project = studyinfo[["Study Identifier"]],
                instrument  = "unknown",
                file = basename(filepath),
                filename_overrides = TRUE);
        }      
    }

    if (!is.list(model_name)) {
        # is character name, convert to a tuple list
        model_name = list(
            positive = `${model_name}_POS`,
            negative = `${model_name}_NEG`
        );
    }

    insert_into(model_name$positive, "POS");
    insert_into(model_name$negative, "NEG");
}