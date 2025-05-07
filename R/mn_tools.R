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

#' Make metabolights project imports into the spectrum data pool system
#' 
#' @param sample_metadata the file path to the sample files' metadata text file.
#' @param repo a mysql connection to the spectrum data pool system, should be a clr object that constructed via the ``open_mnlink`` function.
#' @param investigation the file path to the metabolights project ``i_Investigation.txt`` metadata file. for extract of the metadata of the study.
#' @param model_name a tuple list of the spectrum cluster model for extract the current project rawdata its msn spectrum into the given spectrum cluster model.
#'     should be a tuple list that contains two model name reference: positive/negative for the positive and negative polarity mode of the spectrum data.
#'     or it could be a character name of the model, the POS/NEG suffix for the corresponding positive/negative polarity spectrum cluster model will be 
#'     tagged to the model character name and construct a tuple list for the function.
#' 
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