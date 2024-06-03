$(document).ready(function () {
     listele();

    $("#folderForm").submit(function (e) {
        e.preventDefault();
        var visable = $("#guncelle").is(":visible");
        if (visable) {
            guncelle();
        } else {
            ekle();
        }
    });
});

var selectedFolderId = null;
var baseurl = "http://localhost:5287/";
var gettoken = localStorage.getItem("token");

function listele() {
    $.ajax({
        type: "GET",
        url: baseurl + "api/Folders",
        contentType: "application/json",
        headers: {
            "Authorization": "Bearer " + gettoken
        },
        success: function (response) {
            console.log(response);
            var html = "";
            $.each(response, function (index, item) {
                html += "<tr>";
                html += "<td>" + item.id + "</td>";
                html += "<td>" + item.folderName + "</td>";
                html += "<td>" + new Date(item.created).toLocaleString() + "</td>";
                html += "<td>" + new Date(item.updated).toLocaleString() + "</td>";
                html += "<td>";
                html += "<button class='btn btn-primary' onclick='duzenle(" + item.id + ", \"" + item.folderName + "\")'>Edit</button> ";
                html += "<button class='btn btn-danger' onclick='sil(" + item.id + ")'>Delete</button>";
                html += "</td>";
                html += "</tr>";
            });
            $("#foldersTable tbody").html(html);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + status + " " + error);
        }
    });
}

function sil(id) {
    if (!confirm("Are you sure you want to delete this folder?")) {
        return;
    }

    $.ajax({
        type: "DELETE",
        url: baseurl + "api/Folders/" + id,
        contentType: "application/json",
        headers: {
            "Authorization": "Bearer " + gettoken
        },
        success: function (response) {
            listele();
            $("#divResult").show().removeClass().addClass("alert alert-success").html("Deletion Successful").fadeOut(5000);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + status + " " + error);
        }
    });
}

function ekle() {

    var input = document.getElementById('folderName');
    var file = input.files[0];

    var formData  = new FormData();
    formData.append('FolderName', file);


    $.ajax({
        type: "POST",
        url: baseurl + "api/Folders",
        data: formData,
        contentType: false,
        processData: false,
        headers: {
            "Authorization": "Bearer " + gettoken
        },
        success: function (response) {
            if(response == "hata"){
                alert("hata!");
                
            }

            if(response != "hata"){
                console.log(response);
               
                
            }
        },
        error: function (xhr, status, error) {
           console.log(error);
        }
    });
}





function duzenle(id, folderName) {
    $("#folderId").val(id);
    $("#folderName").val(folderName);
    $("#ekle").hide();
    $("#guncelle").show();
}

function guncelle() {
    var FolderDto = {
        id: $("#folderId").val(),
        folderName: $("#folderName").val()
    };
    var gettoken = localStorage.getItem("token");
    $.ajax({
        type: "PUT",
        url: baseurl + "api/Folders",
        contentType: "application/json",
        data: JSON.stringify(FolderDto),
        headers: {
            "Authorization": "Bearer " + gettoken
        },
        success: function (response) {
            $("#folderForm")[0].reset();
            $("#ekle").show();
            $("#guncelle").hide();
            listele();
            $("#divResult").show().removeClass().addClass("alert alert-success").html("Update Successful").fadeOut(5000);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + status + " " + error);
        }
    });
}