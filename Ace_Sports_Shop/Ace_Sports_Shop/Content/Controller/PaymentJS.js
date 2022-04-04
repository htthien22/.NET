function Payment() {
    //var color = $('#color').find(":selected").text();
    //var size = $('#size').find(":selected").text();
    var name = $('#NameShip').val();
    var phone = $('#PhoneShip').val();
    var email = $('#EmailShip').val();
    var address = $('#AddShip').val() + "," + $('#Add2Ship').val() +","+ $('#Add3Ship').val();
    var check = $("#CommentShip").val();
    var payment = $('input[name="option_payment"]:checked').val();
    var Bank = $('input[name="bankcode"]:checked').val();
    var price = document.getElementById('totalprice').innerText
    $.ajax({
        type: 'POST',
        url: '/Cart/Payment',
        dataType: 'json',
        data: { address: address, mobile: phone, shipName: name, email: email, total: price, check: check, BankCode: Bank, paymentMethod: payment },
        success: function (response) {
            if (response.status == true) {
                swal("Đặt hàng thành công", {
                    icon: "success",
                    buttons: {
                        cancel: "Trở Về",
                        catch: {
                            text: "TIẾP TỤC MUA HÀNG",
                            value: "catch",
                        },
                    },
                })
                    .then((value) => {
                        switch (value) {
                            case "catch":
                                window.location.href = "/Home"
                                break;
                            default: window.location.href = window.location.href

                        }
                    });
            } else {
                swal(response.noti)
            }

        }
    });
}