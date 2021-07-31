<template>
  <div :class="['dropdown d-none', dd_dropdown]" :Url = "Url" :itemId = "itemId" :itemName = "itemName">
    <div class="dropdown-item" v-for= "item in items" :key="item[itemId]"  :Value = "item[itemId]" >
      <div class="dropdown-icon"></div>
      <div class="dropdown-text">{{item[itemName]}}</div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "BaseDropdownDetail",

  data(){
    return{
      items: []
    }
  },

  props: {
    dd_dropdown: String,
    Url : String,
    itemId: String,
    itemName: String,
  },
  created() {
    let me = this;
    axios
      .get(`http://cukcuk.manhnv.net/${me.Url}`)
      .then((res) => {
        me.items = res.data;
      })
      .catch((res) => {
        console.log(res);
      });
  },
};
</script>

<style></style>
