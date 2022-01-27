
function addcartHome(productID) {
    $.ajax({
        url: '/Cart/Additem?productID=' + productID + '&quantity=1',
        type: 'GET',
        success: function (data) {
            alertify.success(" Đã thêm vào  GIỎ HÀNG(" + data.cartCount + ")");
        }
    })
}
function thanhtoan() {
    $.ajax({
        url: '/thanh-toan',
        type: 'GET',
        success: function (data) {
            alertify.error("Bạn phải đăng nhập");
        }
    })
}