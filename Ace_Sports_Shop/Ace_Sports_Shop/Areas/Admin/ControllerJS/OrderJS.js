//function CreateOrder() {
//    var name = $('#txtName').val();
//    var mobile = $('#txtMobile').val();
//    var email = $('#txtEmail').val();
//    var content = $('#txtContent').val();
//    $('#myModal').modal('show');
//    $.ajax({
//        url: '/Home/CreateOrder',
//        type: 'POST',
//        dataType: 'json',
//        data: {
//            name: name,
//            mobile: mobile,
//            email: email,
//            content: content
//        },
//        success: function (res) {
//            if (res.status == true) {
//                swal("Thông báo", "Gửi yêu cầu thành công", "success");
//                resetForm();
//            } else {
//                swal("Thông báo", "Thêm đơn hàng thất bại!", "error");
//            }
//        }
//    });

//}

//order detail
function OrderDetails(ido) {

    $.ajax({
        
        url: '/Admin/Order/Details',
        type: 'POST',
        dataType: 'json',
        data: {
            ID: ido,
        },
        success: function (res) {
            if (res.status == true) {                                            
                $('#myModal').modal('show');
                $('#idd').text(res.ID);
                $('#color').text(res.Color);
                $('#size').text(res.Size);
                $('#qty').text(res.Qty);
                $('#price').text(res.Price+" Vnđ ");

            } else {
                
                swal("Thông báo", "Thêm đơn hàng thất bại!", "error");
            }
        }
    });
  
}

//delete order
function del(id, name) {
    swal({
        title: "Bạn chắc chắn muốn xóa đơn hàng này?",
        text: name,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Order/Delete",
                    dataType: "json",
                    data: { ID: id },
                    success: function (response) {
                        swal("Xóa thành công!", {
                            icon: "success",
                        }).then(function () {
                            window.location.href = window.location.href
                        });
                    },
                    failure: function (response) {
                        swal(response.responseText);
                    },
                    error: function (response) {
                        swal(response.responseText);
                    }
                });

            }
        });
}

//edit=>GET order
function OrderEdit(ido) {
    $.ajax({

        url: '/Admin/Order/GetOrder',
        type: 'POST',
        dataType: 'json',
        data: {
            ID: ido,
        },
        success: function (res) {
            if (res.status == true) {
                $('#myModalEdit').modal('show');
                $('#edtidd').val(res.ID);
                $('#edtname').val(res.Name);
                $('#edtsdt').val(res.Phone);
                $('#edtadd').text(res.Add);

            } else {

                swal("Thông báo", "Thêm đơn hàng thất bại!", "error");
            }
        }
    });

}

//edit post
function Edit() {
    var ido = $('#edtidd').val();
    var name = $('#edtname').val();
    var phone = $('#edtsdt').val();
    var add = $('#edtadd').val();  
    $.ajax({
        url: '/Admin/Order/editOrder',
        type: 'POST',
        dataType: 'json',
        data: {
            ID: ido,
            Name: name,
            Add: add,
            Phone: phone
        },
        success: function (res) {
            if (res.status == true) {
                swal("Cập nhật thông tin đơn hàng thành công!!", {
                    icon: "success",
                }).then(function () {
                    $('#myModalEdit').modal('hide');
                    window.location.href = window.location.href
                });
            } else {
                swal("Thông báo", "Thêm đơn hàng thất bại!", "error");
            }
        }
    });

}

//export to excel
function Excel() {
    swal({
        title: "Bạn muốn xuất toàn bộ thông tin đơn hàng ra file Excel?",
        text: name,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    type: "POST",
                    url: "/Order/ExcelExport",
                    cache: false,
                    success: function (data) {
                        window.location = '/Order/Download';

                    },
                    error: function (data) {
                        swal("", "Xuất file thất bại!", 'error');
                    }
                });

            }
        });  
}