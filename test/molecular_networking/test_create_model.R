require(mzkit_hpc);
require(graphQL);

imports "spectrumPool" from "mzDIA";

let repo = open_mnlink(user = "xieguigang", passwd = 123456, host = "192.168.3.15", port = 3306);

print(repo);

let model = repo |> createMysqlPool(level = 0.9,
                        split = 9,
                        name = "no_named",
                        desc = "no_information");

print(model |> model_id());