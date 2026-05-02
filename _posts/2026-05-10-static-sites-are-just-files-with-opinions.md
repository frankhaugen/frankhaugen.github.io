---
title: Static sites are just files with opinions
tags:
  - Jekyll
  - GitHub-Pages
  - meta
description: GitHub Pages + Jekyll facts — dependency pins, plugin whitelist, and when boring beats trendy.
---

I have shipped enough SPAs to know I do not want a SPA for a personal site that updates when I remember Markdown exists.

## Facts: how GitHub Pages builds Jekyll

GitHub Pages runs Jekyll on every push to the publishing branch with a **pinned dependency set** (Ruby gem versions).[^versions] That matters because:

- You cannot rely on arbitrary Ruby gems—only **whitelisted plugins** ship on github.io unless you switch to GitHub Actions as a custom builder.
- Local `Gemfile` versions should track the published matrix when debugging "works on my laptop."

Default GitHub Pages docs describe turning on Pages, choosing branch/source, and optional custom domains / HTTPS enforcement.[^pages-docs]

Jekyll itself documents configuration (`_config.yml`), collections, posts naming, and Liquid templating—the primitives this site uses for `_layouts` + `_posts`.[^jekyll-docs]

## What this stack is

- **Jekyll** because GitHub Pages runs it without me babysitting a build matrix (default pipeline).
- **No bought theme** because I wanted a dark sci-fi shell without fighting someone else's Sass variables for three evenings.
- **Markdown in `_posts`** because the only workflow I will stick to is "drop a file and push."

## What this stack is not

It is not a comment system. It is not real-time. It is not "edge compute" unless you squint at a CDN and call that philosophy.

## When a SPA *is* justified

Reach for heavier client bundles when you need authenticated interactive dashboards, collaborative editors, or offline-first complexity—none of which describe "here are my repos and rants."

## The hot take

If your blog engine needs more maintenance than your blog posts, you picked the wrong engine. I would rather write another rant about `Channel<T>` than tune webpack.

## TL;DR

Static sites are boring technology — which is exactly why they are still here when your JavaScript framework of the year graduates to meme status.

[^versions]: GitHub Docs — *Dependency versions* for GitHub Pages / Jekyll ecosystem. https://pages.github.com/versions/

[^pages-docs]: GitHub Docs — *GitHub Pages*. https://docs.github.com/pages

[^jekyll-docs]: Jekyll — official documentation. https://jekyllrb.com/docs/

## References

- [GitHub Pages dependency versions](https://pages.github.com/versions/)
- [GitHub Docs — GitHub Pages](https://docs.github.com/pages)
- [Jekyll documentation](https://jekyllrb.com/docs/)
- [GitHub Docs — Using Jekyll with Pages](https://docs.github.com/pages/setting-up-a-github-pages-site-with-jekyll)
