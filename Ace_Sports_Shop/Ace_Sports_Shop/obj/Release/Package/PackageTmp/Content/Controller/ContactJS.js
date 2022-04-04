function Contact() {
    var name = $('#txtName').val();
    var mobile = $('#txtMobile').val();
    var email = $('#txtEmail').val();
    var content = $('#txtContent').val();
    if (name != "" && mobile != "" && email != "" && content != "") {
        $.ajax({
            url: '/Contact/Send',
            type: 'POST',
            async: false,
            cache: false,
            timeout: 30000,
            dataType: 'json',
            data: {
                name: name,
                mobile: mobile,
                email: email,
                content: content
            },
            success: function (res) {
                if (res.status == true) {
                    swal("Thông báo", "Gửi yêu cầu thành công", "success");
                    resetForm();
                } else {
                    swal("Thông báo", "Gửi yêu cầu không thành công! Xin vui lòng kiểm tra lại", "error");
                }
            }
        });
        
    }      
   else{
        swal("Thông báo", "Không được bỏ trống!", "error");
    }

}
function resetForm () {
    $('#txtName').val('');
    $('#txtMobile').val('');
    $('#txtEmail').val('');
    $('#txtContent').val('');
}