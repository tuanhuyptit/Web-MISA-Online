<template >
    <div class="button" :id = "id">
      <div :class="['button-icon', iconName]"></div>
      <div class="button-text">{{ buttonText }}</div>
    </div>
</template>

<script>
export default {
  name: "BaseButton",
 
  props: {
    id: String,
    subClass: String,
    iconName: String,
    buttonText: String,
  },
};
</script>

<style >
</style>