//add to cart
function AddItem(id) {
    var color = $('#color').find(":selected").text();
    var size = $('#size').find(":selected").text();
    var sl = $("#qty").val();
    if (sl > 999||sl<1) {
        swal("Thông báo","Số lượng hàng của sản phẩm không đủ", "error");
    }else {
    $.ajax({
        type: 'POST',
        url: '/Cart/Additem',
        dataType: 'json',
        data: { ProductID: id, Quantity: sl, Color: color, Size: size },
        success: function (response) {
            if (response.status == true) {
                swal("THÊM VÀO GIỎ HÀNG THÀNH CÔNG", {
                    icon: "success",
                    buttons: {
                        cancel: "Trở Về",
                        catch: {
                            text: "XEM GIỎ HÀNG",
                            value: "catch",
                        },
                    },
                })
                    .then((value) => {
                        switch (value) {
                            case "catch":
                                window.location.href="/Cart"
                                break;
                            default: window.location.href = window.location.href
                                
                        }
                    });
            }
            else {
                $('#ErrMD').modal('show');
            }

        }
        });
    }
}


//edit cart item
function UpdateQuantity(id) {
    var sl = $("#pro_" + id).val();
    $.ajax({
        type: 'POST',
        url: '/Cart/UpdateQuantity',
        dataType: 'json',
        data: { ProductID: id, Quantity: sl },
        success: function (response) {
            if (response.status == true) {

                window.location.href = window.location.href
            }
            else {
                console.log(response.mess)
            }

        }
    });
}


//delte cart item
function del(id, name) {
    swal({
        title: "Bạn chắc chắn muốn bỏ sản phẩm này?",
        text: name,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    type: "POST",
                    url: "/Cart/Del",
                    dataType: "json",
                    data: { ProductID: id },
                    success: function (response) {
                        swal("Xóa thành công!", {
                            icon: "success",
                        }).then(function () {
                            window.location.href = window.location.href
                        });

                    },
                    failure: function (response) {
                        alert(response.responseText);
                    },
                    error: function (response) {
                        alert(response.responseText);
                    }
                });

            }
        });
}