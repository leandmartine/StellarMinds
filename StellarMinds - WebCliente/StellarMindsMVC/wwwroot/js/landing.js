/**
 * StellarMinds Landing — scroll reveals, nav, particles, counters
 */
(function () {
  "use strict";

  const nav = document.getElementById("landingNav");
  const toggle = document.getElementById("navToggle");
  const mobileMenu = document.getElementById("mobileMenu");
  const prefersReduced =
    window.matchMedia && window.matchMedia("(prefers-reduced-motion: reduce)").matches;

  // Sticky nav style on scroll
  function onScrollNav() {
    if (!nav) return;
    if (window.scrollY > 40) nav.classList.add("scrolled");
    else nav.classList.remove("scrolled");
  }

  window.addEventListener("scroll", onScrollNav, { passive: true });
  onScrollNav();

  // Mobile menu
  if (toggle && mobileMenu) {
    toggle.addEventListener("click", function () {
      const open = mobileMenu.classList.toggle("open");
      toggle.classList.toggle("open", open);
      toggle.setAttribute("aria-expanded", open ? "true" : "false");
    });

    mobileMenu.querySelectorAll("a").forEach(function (link) {
      link.addEventListener("click", function () {
        mobileMenu.classList.remove("open");
        toggle.classList.remove("open");
        toggle.setAttribute("aria-expanded", "false");
      });
    });
  }

  // Scroll reveal
  const revealEls = document.querySelectorAll(".reveal");
  if (revealEls.length && "IntersectionObserver" in window) {
    const io = new IntersectionObserver(
      function (entries) {
        entries.forEach(function (entry) {
          if (entry.isIntersecting) {
            entry.target.classList.add("visible");
            io.unobserve(entry.target);
          }
        });
      },
      { threshold: 0.12, rootMargin: "0px 0px -40px 0px" }
    );
    revealEls.forEach(function (el) {
      io.observe(el);
    });
  } else {
    revealEls.forEach(function (el) {
      el.classList.add("visible");
    });
  }

  // Animated counters
  function animateCounter(el, target, duration) {
    const start = performance.now();
    const isFloat = String(target).includes(".");
    function frame(now) {
      const t = Math.min(1, (now - start) / duration);
      const eased = 1 - Math.pow(1 - t, 3);
      const value = target * eased;
      el.textContent = isFloat ? value.toFixed(1) : Math.round(value).toString();
      if (t < 1) requestAnimationFrame(frame);
      else el.textContent = isFloat ? Number(target).toFixed(1) : String(target);
    }
    requestAnimationFrame(frame);
  }

  document.querySelectorAll("[data-count]").forEach(function (el) {
    const target = parseFloat(el.getAttribute("data-count") || "0");
    if (prefersReduced) {
      el.textContent = String(target);
      return;
    }
    let done = false;
    if ("IntersectionObserver" in window) {
      const io = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
          if (entry.isIntersecting && !done) {
            done = true;
            animateCounter(el, target, 1400);
            io.disconnect();
          }
        });
      }, { threshold: 0.4 });
      io.observe(el);
    } else {
      animateCounter(el, target, 1400);
    }
  });

  // Floating particles
  if (!prefersReduced) {
    const count = Math.min(28, Math.floor(window.innerWidth / 40));
    for (let i = 0; i < count; i++) {
      const p = document.createElement("span");
      p.className = "particle";
      p.style.left = Math.random() * 100 + "vw";
      p.style.animationDuration = 12 + Math.random() * 18 + "s";
      p.style.animationDelay = Math.random() * 12 + "s";
      p.style.opacity = String(0.2 + Math.random() * 0.5);
      p.style.width = p.style.height = 2 + Math.random() * 3 + "px";
      document.body.appendChild(p);
    }
  }

  // Smooth close video when tab hidden (save battery)
  document.querySelectorAll("video").forEach(function (video) {
    document.addEventListener("visibilitychange", function () {
      if (document.hidden) video.pause();
      else if (video.hasAttribute("autoplay")) {
        video.play().catch(function () { /* autoplay blocked */ });
      }
    });
  });
})();
