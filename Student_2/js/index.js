// Loading screen
    window.addEventListener('load', function () {
      const loading = document.getElementById('loading');
      setTimeout(() => {
        loading.classList.add('hide');
        setTimeout(() => {
          loading.style.display = 'none';
        }, 500);
      }, 1000);
    });

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
      anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
          window.scrollTo({
            top: target.offsetTop - document.querySelector('.navbar').offsetHeight,
            behavior: 'smooth'
          });
        }
      });
    });

    // Navbar background on scroll
    window.addEventListener('scroll', function () {
      const navbar = document.querySelector('.navbar');
      if (window.scrollY > 100) {
        navbar.style.background = 'rgba(255,255,255,0.98)';
        navbar.style.boxShadow = '0 2px 20px rgba(0,0,0,0.1)';
      } else {
        navbar.style.background = 'rgba(255,255,255,0.95)';
        navbar.style.boxShadow = '0 1px 3px rgba(0,0,0,0.1)';
      }
    });

    // Scroll animations
    const observerOptions = {
      threshold: 0.1,
      rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          entry.target.style.animationDelay = `${Array.from(entry.target.parentNode.children).indexOf(entry.target) * 0.1}s`;
          entry.target.classList.add('animate-on-scroll');
        }
      });
    }, observerOptions);

    document.querySelectorAll('.feature-card').forEach(card => {
      observer.observe(card);
    });

    // Rolling number animation
    function animateNumbers() {
      const counters = document.querySelectorAll('.stat-number');
      const speed = 200; // lower = faster

      counters.forEach(counter => {
        const target = +counter.getAttribute('data-target');
        const suffix = counter.textContent.replace(/[0-9]/g, ''); 
        counter.textContent = '0' + suffix;

        const updateCount = () => {
          const current = +counter.textContent.replace(/\D/g, '');
          const increment = Math.ceil(target / speed);

          if (current < target) {
            counter.textContent = `${current + increment}${suffix}`;
            setTimeout(updateCount, 20);
          } else {
            counter.textContent = `${target}${suffix}`;
          }
        };

        updateCount();
      });
    }

    // Trigger when stats come into view
    const statsSection = document.querySelector('.stats');
    const statsObserver = new IntersectionObserver(entries => {
      if (entries[0].isIntersecting) {
        animateNumbers();
        statsObserver.disconnect();
      }
    }, { threshold: 0.5 });

    statsObserver.observe(statsSection);

    // World Food Day Countdown
    function updateCountdown() {
      const now = new Date();
      const year = now.getMonth() === 9 && now.getDate() > 16 ? now.getFullYear() + 1 : now.getFullYear();
      const eventDate = new Date(`October 16, ${year} 00:00:00`).getTime();
      const currentTime = now.getTime();
      const diff = eventDate - currentTime;

      if (diff < 0) {
        document.getElementById("countdown").innerHTML = "<i class='fas fa-party-horn'></i> Happy World Food Day!";
        return;
      }

      const days = Math.floor(diff / (1000 * 60 * 60 * 24));
      const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      const mins = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
      const secs = Math.floor((diff % (1000 * 60)) / 1000);

      document.getElementById("countdown").innerHTML =
        `${days}d ${hours}h ${mins}m ${secs}s`;
    }

    setInterval(updateCountdown, 1000);
    updateCountdown();

    // Mobile menu toggle
    const mobileMenu = document.querySelector('.mobile-menu');
    const navLinks = document.querySelector('.nav-links');

    mobileMenu.addEventListener('click', () => {
      navLinks.classList.toggle('active');
      mobileMenu.classList.toggle('active');
    });