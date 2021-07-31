<template>
 
    <div class="select">
      <div class="inp">{{DropdownText}}</div>
      <div class="X"></div>
      <div class="select-arrow rot-0"></div>
    </div>

</template>

<script>
export default {
  name: "BaseDropdown",
  props: {   
    DropdownText: String,
  },
};
</script>

<style></style>
