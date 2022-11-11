
$("#formComment").submit(function (e) {
    e.preventDefault()
/*    const a = $(this).parents(".posts")*/
    const post = $(this).parents(".posts").find(".comms")
    let formData = new FormData(this)
    $.ajax({
        type: "POST",
        url: e.target.action,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: (res) => {
            this.reset()
            $(post).prepend(
                `   <div class="div4">
                    <div class="d-flex justify-content-between">
                    <div class="d-flex" style="align-items: center;">
                        <a href="/home/profil/@com.CommentUser?UserNickName=${$('#profileimage').attr('alt')}">
                            <img src="${$('#profileimage').attr('src')}" class="pp">
                        </a>
                            <a href="/home/profil/@com.CommentUser?UserNickName=${$('#profileimage').attr('alt')}" style="color:black"> <h5>${$('#profileimage').attr('alt')}</h5> </a>
                    </div>
                    <div>
                        <button class="btn pt-1 fs-5"><a href="/Home/DeleteComment/@com.CommentId"><i class="fa-solid fa-xmark"></i></a></button>
                    </div>
                </div>
                <p>${res.commentDescription}<br /> <br /><span> <b>
                </p>
                <div>

                `
            )
        },
        error: function () {

        }
    })
})
$(document).ready(function () {
    $('.comms').on('click', '.removeComment', function () {
        const btn = $(this)
        $.post('/Home/DeleteComment/' + $(this).attr('commentid'), function (res) {
            $(btn).parents('.div4').remove();
        })
    })

})
$(document).ready(function () {
    $('.div1').on('click', '.LikeNumber', function () {
        const icn = $(this)
        $.post('/Home/PostLike/' + $(this).attr('LikePostId'), function (res) {
            if ($(icn).find('i').hasClass('fa-solid')) {
                $(icn).find('span').text($(icn).text()-1)
            } else {
                $(icn).find('span').text(Number($(icn).text()) + 1)
            }
            $(icn).find('i').toggleClass('text-danger').toggleClass('fa-solid')
        })
    })

})

$(".flw").click(function () {
    if ($(this).text() == "Follow") {
        $.post("/Home/SendFollow/" + $(this).attr("udi"))
        $(this).text("cancel");
    } else {
        $.post("/Home/RemoveFollow/" + $(this).attr("udi"))
        $(this).text("Follow");
    }
})

$(".acceptBtnn").click(function () {
    $.post("/Home/AcceptFollow/" + $(this).attr("udi"))
    $(this).parents(".istek").remove();
})

$(".declineBtn").click(function () {
    $.post("/Home/FollowDelete/" + $(this).attr("udi"))
    $(this).parents(".istek").remove();
})

$(".searchfriend input").keyup(function () {
    $(".searchinputdata").empty()
    if ($(this).val() !="") {
        $.post("/Home/Search?q=" + $(this).val(), function (res) {
            $(res).each(function () {
                $(".searchinputdata").append(
                    `
                     <div class="searchphoto my-2">
                        <img src="/img/${this.userProfilePhotoUrl}" />
                        <a href="/home/profil?UserNickName=${this.userNickName}" class="my-2">${this.userNickName}</a>
                    </div>
                `)
            })
        })
    }
})

