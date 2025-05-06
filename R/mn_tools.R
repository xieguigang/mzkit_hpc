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