require(mzkit_hpc);
require(graphQL);

imports "spectrumPool" from "mzDIA";

let repo = open_mnlink(user = "xieguigang", passwd = 123456, host = "127.0.0.1", port = 3306);

print(repo);

let model = repo |> openMysqlPool(model_id = 1);
let rawfilepath = "G:\\lxy-CID30.mzML";
let rawdata = open.mzpack(rawfilepath);

repo 
|> project_context("XXXXXXXXXX")
|> group_info("XXXX")
|> file_info(rawfilepath)
;

model |> addPool( x = ms2_peaks(rawdata),
                         biosample  = "unknown",
                         organism  = "unknown",
                         project = "unknown",
                         instrument  = "unknown",
                         file = basename(rawfilepath),
                         filename_overrides = TRUE);
