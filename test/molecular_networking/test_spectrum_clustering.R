require(mzkit_hpc);
require(graphQL);

imports "spectrumPool" from "mzDIA";

let repo = open_mnlink(user = "xieguigang", passwd = 123456, host = "192.168.3.15", port = 3306);

print(repo);

let model = repo |> openMysqlPool(model_id = 1);
let rawfilepath = "G:\\lxy-CID30.mzML";
let rawdata = open.mzpack(rawfilepath);

model |> addPool( x = ms2_peaks(rawdata),
                         biosample  = "unknown",
                         organism  = "unknown",
                         project = "unknown",
                         instrument  = "unknown",
                         file = basename(rawfilepath),
                         filename_overrides = TRUE);
