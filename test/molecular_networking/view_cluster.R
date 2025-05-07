require(mzkit_hpc);

let repo = open_mnlink(user = "xieguigang", passwd = 123456, host = "127.0.0.1", port = 3306);
let cid = 68;
let spectrum = repo |> cluster_spectrum(cluster_id = cid);
let i = 0;

imports "visual" from "mzplot";

for(let spectral in spectrum) {
    print(spectral);
    bitmap(file = file.path(@dir, `cluster_${cid}`, `${[spectral]::lib_guid}.jpg`)) {
        plot(spectral);
    }    
}
