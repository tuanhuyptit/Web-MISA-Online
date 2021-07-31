<template>
  <div class="header">
            <div class="header-left">
                <div class="toggle"></div>
                <div class="logo"></div>
            </div>
            <div class="header-right">
                <div class="restaurant-selector">
                    <div class="restaurant-name"><b>Nhà hàng Biển Đông</b></div>
                    <div class="arrow"></div>
                </div>
                <div class="user">
                    <div class="user-avatar"></div>
                    <div class="user-fullname">Đỗ Minh Ngọc</div>
                    <div class="user-function"></div>
                </div>
            </div>
        </div>
</template>

<script>
export default {
    name: 'Header'
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    @import url("../../css/layout/header.css")
</style>
