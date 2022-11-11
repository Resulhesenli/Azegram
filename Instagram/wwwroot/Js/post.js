document.querySelector('#postsekilselect').addEventListener('change', function() {
    let item=document.createElement('div');
    item.className='item'
    let sekil=document.createElement('img');
    sekil.src=URL.createObjectURL(this.files[0])
    item.appendChild(sekil)
    item.appendChild(this.cloneNode(true))
    this.value=null;
    item.addEventListener('click',function(){
        this.remove();
    })
    document.querySelector('.parea').appendChild(item);
})