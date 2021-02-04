package com.realt.statsapi.controllers;

import com.realt.statsapi.data.Search;
import com.realt.statsapi.data.SearchRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Collection;

@Controller
@RequestMapping("search")
public class SearchController {

    private static final Logger log = LoggerFactory.getLogger(SearchController.class);

    @Autowired
    private SearchRepository searchRepository;

    @GetMapping("/saved")
    public @ResponseBody Search[] getSavedSearches() {

        log.info("Loading searches");
        Collection<Search> data = searchRepository.findAll();
        log.info("Loaded searches: %s", data.size());

        return data.toArray(new Search[0]);
    }
}