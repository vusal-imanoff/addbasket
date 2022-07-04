$(document).ready(function () {

       // modal quantity 
    //$('.adds').click(function () {
    //    if ($('.modalcount').val()) {
    //        $('.modalcount').val()(+$('.modalcount').val() + 1);
    //    }
    //});
    //$('.subs').click(function () {
    //    if ($('.modalcount').val() > 1) {
    //        if ($('.modalcount').val() > 1) $('.modalcount').val()(+$('.modalcount').val() - 1);
    //        console.log('salam')
    //    }
    //});



document.getElementsByClassName('addbasket')[0].addEventListener('click', (e) => {
        e.preventDefault();
        const pathname = window.location.pathname;
        let id = '';
        const count = document.getElementsByClassName('quantity')[0].children[1].value;

        for (let i = pathname.length - 1; i > 0; i--) {
            if (!isNaN(pathname[i])) {
                id = pathname[i] + id;
            }
            else {
                break;
            }
        }

        const port = window.location.port;

        $.ajax({
            url: "/Basket/AddToBasket",
            type: 'GET',
            data: { id: id, count: count },
            contentType: 'application/json; charset=utf-8',
            dataType: "text",
            success: function (data) {
                document.getElementsByClassName('header-cart')[0].innerHTML = data;
            }
        });

    });

    //$(document).on('click', '.addbasket', function (e) {
    //    e.preventDefault()

    //    fetch($(this).attr('href'))
    //        .then(res => res.text())
    //        .then(data => { $('.header-cart').html(data) });

    //})

    $(document).on('click', '.addtobasket', function (e) {
        e.preventDefault()

        fetch($(this).attr('href'))
        .then(res => res.text())
            .then(data => { $('.header-cart').html(data) });

    })


    $(document).on('click', '.deletebasket', function (e) {
        e.preventDefault()

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => { $('.header-cart').html(data) });

    })

    $(document).on('click', '.deleteproductbasket', function (e) {
        e.preventDefault()

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => { $('.table').html(data) });

    })
    $(document).on('keyup', '.searchinput', function () {
        console.log($(this).val());
        console.log($(this).data("url"))
        let url = $(this).data("url") + '?search=' + $(this).val();
        console.log(url);

        if ($(this).val().trim().length > 2) {

            fetch(url)
                .then(res => res.text())
                .then(data => {
                    console.log(data);
                    $('.searchlist').html(data);
                })

            //Old Partial
            //fetch(url)
            //    .then(res => res.json())
            //    .then(data => {

            //        let listitemts = "";
            //        console.log(data)
            //        for (var i = 0; i < data.length; i++) {
            //            let listitem = `<li class="list-group-item">
            //                                    <img style="width:50px;" src="assets/images/product/${data[i].mainImage}" alt="Alternate Text" />
            //                                    ${data[i].title} 
            //                                </li>`

            //            listitemts += listitem;
            //        }

            //        $('.searchlist').html(listitemts)
            //    });
        }
        else {
            $('.searchlist').html("");
        }

        
    })

    $(document).on('click', '.productModal', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.modal-body').html(data);

                $('.quick-view-image').slick({
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    arrows: false,
                    dots: false,
                    fade: true,
                    asNavFor: '.quick-view-thumb',
                    speed: 400,
                });

                $('.quick-view-thumb').slick({
                    slidesToShow: 4,
                    slidesToScroll: 1,
                    asNavFor: '.quick-view-image',
                    dots: false,
                    arrows: false,
                    focusOnSelect: true,
                    speed: 400,
                });
            })
    })
    let pageindex = parseInt($('.pageindex').val());

    let page = pageindex;

    $(document).on('click', '.loadmore', function (e) {
        e.preventDefault();
        let productCount = parseInt($('.productCount').val());

        page++;
        
        let url = $(this).attr('href') + '?page=' + page;

        if (page <= productCount) {
            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $('.productcontainer').append(data);
                })
        }

        if (page == productCount) {
            $(this).remove();
        }
    })

    console.log(window.location.pathname.split('/')[1]);
    let path = window.location.pathname.split('/')[1];
    let links = $('.header-horizontal-menu .menu-content li');
    console.log(links)

    for (var i = 0; i < links.length; i++) {
        console.log(links[i].children[0].getAttribute('href').split('/')[1])
        let linkpath = links[i].children[0].getAttribute('href').split('/')[1];
        if (path.trim().toLocaleLowerCase() == linkpath.trim().toLocaleLowerCase()) {
            links[i].classList.add('active');
        } else {
            links[i].classList.remove('active');
        }
    }
})